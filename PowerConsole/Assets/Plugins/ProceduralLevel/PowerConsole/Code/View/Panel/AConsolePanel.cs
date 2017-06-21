using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.View
{
	public abstract class AConsolePanel
	{
		protected ConsoleInstance m_Console;

		public AConsolePanel(ConsoleInstance console)
		{
			m_Console = console;
		}
	}
}
