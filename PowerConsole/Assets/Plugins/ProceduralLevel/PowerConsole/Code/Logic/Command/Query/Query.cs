using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class Query
	{
		public string RawQuery = "";
		public readonly Argument Name;
		public List<Argument> Arguments = new List<Argument>();

		public Query(Argument commandName)
		{
			Name = commandName;
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

		public Argument GetArgumentAt(int cursorPosition)
		{
			for(int x = 0; x < Arguments.Count; x++)
			{
				Argument arg = Arguments[x];
				if(arg.Offset > cursorPosition)
				{
					return null;	
				}
				if(arg.Offset+arg.Value.Length < cursorPosition)
				{
					return arg;
				}
			}
			return null;
		}

		public override string ToString()
		{
			return string.Format("[Query: {0}]", RawQuery);
		}
	}
}
