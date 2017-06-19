using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class Console
	{
		private ValueParser m_ValueParser = new ValueParser();
		private QueryParser m_QueryParser = new QueryParser();

		private List<AConsoleCommand> m_Commands = new List<AConsoleCommand>();

		public Console()
		{
			
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

		public List<Query> ParseQuery(string strQuery)
		{ 
			m_QueryParser.Parse(strQuery);
			List<Query> queries = m_QueryParser.Flush();
			return queries;
		}

		public Message Execute(Query query)
		{
			return null;
		}
	}
}
