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
			Console.RepeatCurrentQueries(count-1);
			return null;
		}
	}
}
