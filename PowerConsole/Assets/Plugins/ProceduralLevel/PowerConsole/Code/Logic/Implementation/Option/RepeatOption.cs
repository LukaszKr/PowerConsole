using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class RepeatOption: AConsoleCommand
	{
		public RepeatOption(ConsoleInstance console) 
			: base(console, ELocKey.OptRepeatName, ELocKey.OptRepeatDesc, true)
		{
		}

		public Message Command(int count)
		{
			List<Query> queries = Console.ExecutionStack;
			for(int repeatIndex = 0; repeatIndex < count; repeatIndex++)
			{
				for(int queryIndex = queries.Count-1; queryIndex >= 0; queryIndex--)
				{
					Console.Execute(queries[queryIndex]);
				}
			}
			return null;
		}
	}
}
