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
		public readonly HintManager Hints = new HintManager();
		public readonly CommandNameHint NameHint;

		public readonly InputState InputState;
		public readonly HintState HintState;


		public ConsoleInstance(LocalizationManager localizationProvider)
		{
			Localization = localizationProvider;
			NameHint = new CommandNameHint(m_Commands);

			InputState = new InputState(this);
			HintState = new HintState(this);

			InputState.BindEvents();
			HintState.BindEvents();

			Factory.CreateDefaultCommands(this);
		}

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

		#region Execution
		public void Execute(string strQuery)
		{
			List<Query> queries = ParseQuery(strQuery);
			for(int x = 0; x < queries.Count; x++)
			{
				Query query = queries[x];
				Execute(query);
			}
		}

		public void Execute(Query query)
		{
			OnMessage.Invoke(new Message(EMessageType.Execution, query.RawQuery));
			AConsoleCommand command = FindCommand(query.Name.Value);
			if(command == null)
			{
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.CommandNotFound(query.Name.Value)));
				return;
			}
			CommandMethod method = command.Method;
			if(MapQuery(method, query) && ParseQueryValues(query))
			{
				object[] parsed = query.GetParsedValues();
				Message result = command.Execute(parsed);
				OnMessage.Invoke(result);
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
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.NotEnoughtArguments(method.ParameterCount-e.Parameters.Count)));
				return false;
			}
			catch(NamedArgumentNotFoundException e)
			{
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.NamedArgumentNotFound(e.Name, e.Value)));
				return false;
			}
			catch(TooManyArgumentsException e)
			{
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.TooManyArguments(e.Received, e.Expected)));
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
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.MissingValueParser(e.RawValue, e.ExpectedType)));
				return false;
			}
			catch(InvalidValueFormatException e)
			{
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.InvalidValueFormat(e.RawValue, e.ExpectedType)));
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
		#endregion
	}
}
