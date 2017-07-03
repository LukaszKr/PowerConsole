using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class HintProvider
	{
		private readonly HintManager m_HintsManager = new HintManager();
		private ConsoleInstance m_Console;

		public HintProvider(ConsoleInstance console)
		{
			m_Console = console;
		}

		public AHintIterator GetHintIterator(string input, int cursor, List<ConsoleException> issues = null)
		{
			List<Query> queries;

			try
			{
				queries = m_Console.ParseQuery(input);
			}
			catch(ConsoleException e)
			{
				queries = new List<Query>();
				issues.Add(e);
			}

			Query query = null;
			Argument argument = null;
			AConsoleCommand command = null;

			FindQueryAndArgument(queries, cursor, out query, out argument);

			if(query != null)
			{
				command = m_Console.FindCommand(query.Name.Value);
				MapAndParseQuery(command, query, issues);
			}

			AHint hint = GetHint(command, query, argument);
			if(hint != null)
			{
				if(argument == null)
				{
					argument = new Argument(true);
				}
				return hint.GetIterator(query, argument);
			}

			return null;
		}

		private AHint GetHint(AConsoleCommand command, Query query, Argument argument)
		{
			if(argument != null)
			{
				if(argument.Parameter != null)
				{
					return command.GetHintFor(m_HintsManager, argument.Parameter.Index);
				}
				else if(argument.IsCommandName)
				{
					return (query.IsOption ? m_Console.OptionHint : m_Console.NameHint);
				}
			}
			else
			{
				return m_Console.NameHint;
			}
			return null;
		}

		private void FindQueryAndArgument(List<Query> queries, int cursor, out Query query, out Argument argument)
		{
			for(int x = 0; x < queries.Count; x++)
			{
				query = queries[x];
				argument = query.GetArgumentAt(cursor);
				if(argument != null)
				{
					return;
				}
			}
			query = null;
			argument = null;
		}

		private void MapAndParseQuery(AConsoleCommand command, Query query, List<ConsoleException> issues)
		{
			if(command != null)
			{
				try
				{
					command.Method.MapArguments(query);
				}
				catch(ConsoleException e)
				{
					issues.Add(e);
				}
				try
				{
					m_Console.ParseValues(query);
				}
				catch(ConsoleException e)
				{
					issues.Add(e);
				}
			}
		}
	}
}
