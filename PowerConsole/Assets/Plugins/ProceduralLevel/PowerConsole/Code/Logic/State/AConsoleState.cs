namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class AConsoleState
	{
		public readonly ConsoleInstance Console;

		public AConsoleState(ConsoleInstance console)
		{
			Console = console;
		}

		public abstract void BindEvents();
	}
}
