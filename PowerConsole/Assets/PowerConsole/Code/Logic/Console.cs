using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class Console
	{
		private ValueParser m_ValueParser = new ValueParser();
		private QueryParser m_QueryParser = new QueryParser();

		public Console()
		{
			
		}

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
