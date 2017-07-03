using ProceduralLevel.Common.Event;
using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class ConsoleInstance
	{

		private QueryParser m_QueryParser = new QueryParser();

		private List<AConsoleCommand> m_Commands = new List<AConsoleCommand>();

		public Event<Message> OnMessage = new Event<Message>();

		public readonly LocalizationManager Localization;
		public readonly ValueParser ValueParser = new ValueParser();
		public readonly CommandNameHint NameHint;
		public readonly CommandNameHint OptionHint;

		public readonly InputModule InputModule;
		public readonly HintModule HintModule;
		public readonly HistoryModule HistoryModule;
		public readonly MacroModule MacroModule;

		public bool PrintExecutedCommand = true;

		private List<AConsoleModule> m_Modules;

		public bool Locked { get; private set; }

		public readonly IPersistence Persistence;

		private List<AConsoleCommand> m_DefaultOptions;


		public ConsoleInstance(LocalizationManager localizationProvider, IPersistence persistence)
		{
			Localization = localizationProvider;
			NameHint = new CommandNameHint(m_Commands, false);
			OptionHint = new CommandNameHint(m_Commands, true);

			InputModule = new InputModule(this);
			HintModule = new HintModule(this);
			HistoryModule = new HistoryModule(this);
			MacroModule = new MacroModule(this);

			Persistence = (persistence ??new MockPersistence());

			m_Modules = new List<AConsoleModule>()
			{
				InputModule, HintModule, HistoryModule, MacroModule
			};

			m_DefaultOptions = new List<AConsoleCommand>()
			{
				new RepeatOption(this)
			};

			for(int x = 0; x < m_Modules.Count; x++)
			{
				AConsoleModule module = m_Modules[x];
				module.Read(Persistence);
				module.BindEvents();
			}

			InputModule.SetInput("", 0);
		}

		public void SetupDefault()
		{
			Factory.CreateDefaultCommands(this);
		}

		public void SetLocked(bool locked)
		{
			Locked = locked;
		}

		#region Parsing
		public List<Query> ParseQuery(string strQuery)
		{
			m_QueryParser.Parse(strQuery);
			List<Query> queries = m_QueryParser.Flush();
			return queries;
		}

		public void ParseValues(Query query)
		{
			for(int x = 0; x < query.Arguments.Count; x++)
			{
				Argument argument = query.Arguments[x];
				if(argument.Parameter != null)
				{
					argument.ParseValue(ValueParser);
				}
			}
		}
		#endregion

		#region Execution
		public List<Query> ExecutionStack = new List<Query>();

		public void Execute(string strQuery, bool recordHistory = true)
		{
			if(recordHistory)
			{
				HistoryModule.Add(strQuery);
			}
			List<Query> queries = ParseQuery(strQuery);
			Execute(queries);
		}

		public void Execute(List<Query> queries)
		{
			for(int x = 0; x < queries.Count; x++)
			{
				Query query = queries[x];
				if(!query.IsOption && (x == queries.Count-1 || !queries[x+1].IsOption))
				{
					ExecuteStack(ExecutionStack.Count > 1);
				}
				if(query.IsOption)
				{
					if(ExecutionStack.Count == 0)
					{
						OnMessage.Invoke(new Message(EMessageType.Error, Localization.Get(ELocKey.LogicOptionWithoutCommand)));
						return;
					}
					else
					{
						AConsoleCommand option = query.GetCommand(this);
						AConsoleCommand command = ExecutionStack[0].GetCommand(this);
						if(!command.IsOptionValid(option.GetType()))
						{
							string message = Localization.Get(ELocKey.LogicOptionInvalid, option.Name, command.Name);
							OnMessage.Invoke(new Message(EMessageType.Error, message));
							return;
						}
					}
				}
				ExecutionStack.Add(query);
			}
			ExecuteStack(ExecutionStack.Count > 1);
			ExecutionStack.Clear();
		}

		private void ExecuteStack(bool hasOptions)
		{
			int count = ExecutionStack.Count-1;
			for(int stackIndex = count; stackIndex >= (hasOptions? 1: 0); stackIndex--)
			{
				int lastIndex = ExecutionStack.Count-1;
				Query executedQuery = ExecutionStack[lastIndex];
				ExecutionStack.RemoveAt(lastIndex);
				Execute(executedQuery);
			}
		}

		public void Execute(Query query)
		{
			if(PrintExecutedCommand)
			{
				OnMessage.Invoke(new Message(EMessageType.Execution, query.RawQuery));
			}
			AConsoleCommand command = FindCommand(query.Name.Value);
			if(command == null)
			{
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.Get(ELocKey.LogicCommandNotFound, query.Name.Value)));
				return;
			}
			CommandMethod method = command.Method;
			if(MapQuery(method, query) && ParseQueryValues(query))
			{
				if(Locked && command.ObeyLock)
				{
					OnMessage.Invoke(new Message(EMessageType.Warning, Localization.Get(ELocKey.LogicConsoleLocked)));
					return;
				}

				object[] parsed = query.GetParsedValues();
				Message result = command.Execute(parsed);
				if(result != null)
				{
					OnMessage.Invoke(result);
				}
			}
		}

		private bool MapQuery(CommandMethod method, Query query)
		{
			try
			{
				method.MapArguments(query);
			}
			catch(NotEnoughtArgumentsException e)
			{
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.Get(e)));
				return false;
			}
			catch(NamedArgumentNotFoundException e)
			{
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.Get(e)));
				return false;
			}
			catch(TooManyArgumentsException e)
			{
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.Get(e)));
				return false;
			}
			return true;
		}

		private bool ParseQueryValues(Query query)
		{
			try
			{
				ParseValues(query);
			}
			catch(MissingValueParserException e)
			{
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.Get(e)));
				return false;
			}
			catch(InvalidValueFormatException e)
			{
				OnMessage.Invoke(new Message(EMessageType.Error,  Localization.Get(e)));
				return false;
			}
			return true;
		}
		#endregion

		#region Command Manipulation
		public void AddCommand(AConsoleCommand command)
		{
			AConsoleCommand existing = FindCommand(command.Name);
			if(existing != null)
			{
				throw new DuplicatedCommandException(existing, command);
			}
			if(command.Method == null)
			{
				command.ParseMethod();
			}
			m_Commands.Add(command);
			NameHint.InvalidateCache();
			OptionHint.InvalidateCache();

			for(int x = 0; x < m_DefaultOptions.Count; x++)
			{
				command.AddValidOption(m_DefaultOptions[x].GetType());
			}
		}

		public bool RemoveCommand(AConsoleCommand command)
		{
			return RemoveCommand(command.Name);
		}

		public bool RemoveCommand(string name)
		{
			int index = IndexOfCommand(name);
			if(index >= 0)
			{
				m_Commands.RemoveAt(index);
				NameHint.InvalidateCache();
				OptionHint.InvalidateCache();
				return true;
			}
			return false;
		}

		public AConsoleCommand FindCommand(string name)
		{
			int index = IndexOfCommand(name);
			if(index >= 0)
			{
				return m_Commands[index];
			}
			return null;
		}

		private int IndexOfCommand(AConsoleCommand command)
		{
			return IndexOfCommand(command.Name);
		}

		private int IndexOfCommand(string name)
		{
			for(int x = 0; x < m_Commands.Count; x++)
			{
				AConsoleCommand command = m_Commands[x];
				if(command.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return x;
				}
			}
			return -1;
		}

		public void AddCommonOption(AConsoleCommand command)
		{
			if(command == null || !command.IsOption)
			{
				throw new ArgumentException();
			}
			m_DefaultOptions.Add(command);
		}

		public IEnumerator<AConsoleCommand> GetCommandEnumerator()
		{
			return m_Commands.GetEnumerator();
		}
		#endregion
	}
}
