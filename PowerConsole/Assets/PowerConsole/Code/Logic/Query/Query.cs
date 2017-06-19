using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class Query
	{
		public string RawQuery = "";
		public List<QueryParam> Params = new List<QueryParam>();

		public Query()
		{
		}
	}
}
