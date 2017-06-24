using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class Query
	{
		public string RawQuery = "";
		public readonly string CommandName;
		public List<Argument> Arguments = new List<Argument>();

		public Query(string commandName)
		{
			CommandName = commandName;
		}

		public object[] GetParsedValues()
		{
			object[] parsed = new object[Arguments.Count];
			for(int x = 0; x < parsed.Length; x++)
			{
				Argument argument = Arguments[x];
				int index = argument.Parameter.Index;
				parsed[index] = (argument.Parsed != null? argument.Parsed: argument.Parameter.DefaultValue);
			}
			return parsed;
		}

		public override string ToString()
		{
			return string.Format("[Query: {0}]", RawQuery);
		}
	}
}
