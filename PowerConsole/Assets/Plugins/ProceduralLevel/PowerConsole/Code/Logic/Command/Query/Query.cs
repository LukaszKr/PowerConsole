using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class Query
	{
		public readonly bool IsOption;
		public string RawQuery = "";
		public readonly Argument Name;
		public List<Argument> Arguments = new List<Argument>();

		public Query(Argument commandName, bool isOption)
		{
			Name = commandName;
			IsOption = isOption;
		}

		public AConsoleCommand GetCommand(ConsoleInstance console)
		{
			if(Name.Parsed != null)
			{
				return console.FindCommand(Name.Parsed as string);
			}
			else
			{
				return console.FindCommand(Name.Value);
			}
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
			if(Name.ContainsCursor(cursorPosition))
			{
				return Name;
			}
			for(int x = 0; x < Arguments.Count; x++)
			{
				Argument arg = Arguments[x];
				if(arg.Offset > cursorPosition)
				{
					return null;	
				}
				if(arg.ContainsCursor(cursorPosition))
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
