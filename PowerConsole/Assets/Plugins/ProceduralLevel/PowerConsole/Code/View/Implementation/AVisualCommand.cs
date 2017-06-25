using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.View
{
	public abstract class AVisualCommand: AConsoleCommand
	{
		protected ConsoleView m_View;

		public AVisualCommand(ConsoleView view, string name, string description) : base(view.Console, name, description)
		{
			m_View = view;
		}
	}
}
