using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class RepeatOption: AConsoleCommand
	{
		protected RepeatOption(ConsoleInstance console) 
			: base(console, ELocKey.OptRepeatName, ELocKey.OptRepeatDesc, true)
		{
		}

		public Message Command(int count)
		{
			List<Query> queries = Console.ExecutionStack;
			for(int x = queries.Count-1; x >= 0; x--)
			{
				Console.Execute(queries[x]);
			}
			return null;
		}
	}
}
