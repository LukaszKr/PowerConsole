using ProceduralLevel.Common.Event;
using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class Console
	{
		private ValueParser m_ValueParser = new ValueParser();
		private QueryParser m_QueryParser = new QueryParser();

		private List<AConsoleCommand> m_Commands = new List<AConsoleCommand>();

		public Event<Message> OnMessage = new Event<Message>();

		private ALocalization m_Localization;

		public Console(ALocalization localizationProvider)
		{
			m_Localization = localizationProvider;
		}

		public List<Query> ParseQuery(string strQuery)
		{
			m_QueryParser.Parse(strQuery);
			List<Query> queries = m_QueryParser.Flush();
			return queries;
		}

		public void Execute(Query query)
		{
			OnMessage.Invoke(new Message(EMessageType.Error, query.RawQuery));
			if(query.Arguments.Count > 0)
			{
				string commandName = query.Arguments[0].Value;
				AConsoleCommand command = FindCommand(commandName);
				if(command == null)
				{
					OnMessage.Invoke(new Message(EMessageType.Error, m_Localization.CommandNotFound(commandName)));
				}
				CommandMethod method = command.Method;
				try
				{
					method.Parse(m_ValueParser, query);
				}
				catch(MissingValueParserException e)
				{
					OnMessage.Invoke(new Message(EMessageType.Error, m_Localization.MissingValueParser(e.RawValue, e.ExpectedType)));
				}
				catch(InvalidValueFormatException e)
				{
					OnMessage.Invoke(new Message(EMessageType.Error, m_Localization.InvalidValueFormat(e.RawValue, e.ExpectedType)));
				}
			}
		}

		public void Execute(string strQuery)
		{
			List<Query> queries = ParseQuery(strQuery);
			for(int x = 0; x < queries.Count; x++)
			{
				Query query = queries[x];
				Execute(query);
			}
		}

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
