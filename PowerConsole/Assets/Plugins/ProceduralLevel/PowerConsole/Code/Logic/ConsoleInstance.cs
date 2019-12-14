using ProceduralLevel.Common.Event;
using System;
using System.Collections;
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
				new RepeatOption(this),
				new DelayOption(this)
			};

			for(int x = 0; x < m_Modules.Count; x++)
			{
				AConsoleModule module = m_Modules[x];
				module.Read(Persistence);
				module.BindEvents();
			}

			InputModule.SetInput("", 0);
		}

		public IEnumerator Update()
		{
			if(m_CurrentEnumerator != null)
			{
				object current = m_CurrentEnumerator.Current;
				if(current != null && current.GetType() == typeof(Message))
				{
					OnMessage.Invoke(current as Message);
					m_CurrentEnumerator = null;
				}
				else if(current == null)
				{
					m_CurrentEnumerator = null;
				}
			}
			if(m_CurrentQueries.Count == 0 && m_PendingBatches.Count > 0)
			{
				m_CurrentQueries = m_PendingBatches.Dequeue();
			}
			while(m_CurrentEnumerator == null && m_CurrentQueries.Count > 0)
			{
				m_CurrentEnumerator = ProcessNextQuery();
				if(m_CurrentEnumerator != null)
				{
					return m_CurrentEnumerator;
				}
			}
			return null;
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
		private readonly Queue<Stack<Query>> m_PendingBatches = new Queue<Stack<Query>>();
		private Stack<Query> m_CurrentQueries = new Stack<Query>(); //just to avoid null checks

		private IEnumerator m_CurrentEnumerator = null;
		
		public void Execute(string strQuery, bool recordHistory = true)
		{
			if(recordHistory)
			{
				HistoryModule.Add(strQuery);
			}
			List<Query> queries = ParseQuery(strQuery);
			HandleQueries(queries);
		}

		private void HandleQueries(List<Query> queries)
		{
			bool processingOptions = false;
			Stack<Query> batch = new Stack<Query>();
			for(int x = 0; x < queries.Count; x++)
			{
				Query query = queries[x];
				if(processingOptions &&	!query.IsOption)
				{
					m_PendingBatches.Enqueue(batch);
					batch = new Stack<Query>();
				}
				processingOptions = query.IsOption;

				if(processingOptions)
				{
					if(batch.Count == 0)
					{
						OnMessage.Invoke(new Message(EMessageType.Error, Localization.Get(ELocKey.LogicOptionWithoutCommand)));
						return;
					}
					else
					{
						AConsoleCommand option = query.GetCommand(this);
						AConsoleCommand command = batch.Peek().GetCommand(this);
						if(command == null)
						{
							OnMessage.Invoke(new Message(EMessageType.Error, Localization.Get(ELocKey.LogicCommandNotFound, query.Name.Value)));
						}
						else if(!command.IsOptionValid(option.GetType()))
						{
							string message = Localization.Get(ELocKey.LogicOptionInvalid, option.Name, command.Name);
							OnMessage.Invoke(new Message(EMessageType.Error, message));
							return;
						}
					}
				}
				batch.Push(query);
			}
			if(batch.Count > 0)
			{
				m_PendingBatches.Enqueue(batch);
			}
		}

		private IEnumerator ProcessNextQuery()
		{
			Query query = m_CurrentQueries.Pop();
			if(PrintExecutedCommand && !query.IsOption)
			{
				OnMessage.Invoke(new Message(EMessageType.Execution, query.RawQuery));
			}
			AConsoleCommand command = FindCommand(query.Name.Value);
			if(command == null)
			{
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.Get(ELocKey.LogicCommandNotFound, query.Name.Value)));
				return null;
			}
			CommandMethod method = command.Method;
			if(MapQuery(method, query) && ParseQueryValues(query))
			{
				if(Locked && command.ObeyLock)
				{
					OnMessage.Invoke(new Message(EMessageType.Warning, Localization.Get(ELocKey.LogicConsoleLocked)));
					return null;
				}

				object[] parsed = query.GetParsedValues();
				object result = command.Execute(parsed);

				m_CurrentEnumerator = result as IEnumerator;
				if(m_CurrentEnumerator != null)
				{
					return m_CurrentEnumerator;
				}

				Message message = result as Message;
				if(message != null)
				{
					OnMessage.Invoke(message);
				}
			}
			return null;
		}

		public void RepeatCurrentQueries(int nTimes)
		{
			Queue<Query> originalQueries = new Queue<Query>(m_CurrentQueries);
			for(int x = 0; x < nTimes; ++x)
			{
				foreach(Query query in originalQueries)
				{
					m_CurrentQueries.Push(query);
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
