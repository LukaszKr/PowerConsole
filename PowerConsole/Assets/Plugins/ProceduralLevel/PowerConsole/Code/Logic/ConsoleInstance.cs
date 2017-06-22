using ProceduralLevel.Common.Event;
using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class ConsoleInstance
	{
		private ValueParser m_ValueParser = new ValueParser();
		private QueryParser m_QueryParser = new QueryParser();

		private List<AConsoleCommand> m_Commands = new List<AConsoleCommand>();

		public Event<Message> OnMessage = new Event<Message>();

		public readonly ALocalization Localization;

		public ConsoleInstance(ALocalization localizationProvider)
		{
			Localization = localizationProvider;
		}

		public List<Query> ParseQuery(string strQuery)
		{
			m_QueryParser.Parse(strQuery);
			List<Query> queries = m_QueryParser.Flush();
			return queries;
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
			AConsoleCommand command = FindCommand(query.CommandName);
			if(command == null)
			{
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.CommandNotFound(query.CommandName)));
				return;
			}
			CommandMethod method = command.Method;
			MapQuery(method, query);
			ParseQueryValues(query);
			object[] parsed = query.GetParsedValues();
			Message result = command.Execute(parsed);
			OnMessage.Invoke(result);
		}

		private void MapQuery(CommandMethod method, Query query)
		{
			try
			{
				method.MapArguments(query);
			}
			catch(NamedArgumentNotFoundException e)
			{
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.NamedArgumentNotFound(e.Name, e.Value)));
			}
			catch(TooManyArgumentsException e)
			{
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.TooManyArguments(e.Received, e.Expected)));
			}
		}

		private void ParseQueryValues(Query query)
		{
			try
			{
				for(int x = 0; x < query.Arguments.Count; x++)
				{
					Argument argument = query.Arguments[x];
					argument.Parsed = m_ValueParser.Parse(argument.Parameter.Type, argument.Value);
				}
			}
			catch(MissingValueParserException e)
			{
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.MissingValueParser(e.RawValue, e.ExpectedType)));
			}
			catch(InvalidValueFormatException e)
			{
				OnMessage.Invoke(new Message(EMessageType.Error, Localization.InvalidValueFormat(e.RawValue, e.ExpectedType)));
			}
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
