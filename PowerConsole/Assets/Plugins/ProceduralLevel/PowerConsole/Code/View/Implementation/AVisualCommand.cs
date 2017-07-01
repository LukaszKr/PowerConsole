using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.View
{
	public abstract class AVisualCommand: AConsoleCommand
	{
		protected ConsoleView m_View;

		public AVisualCommand(ConsoleView view, string name, string description, bool isOption = false) 
			: base(view.Console, name, description, isOption)
		{
			m_View = view;
		}

		public AVisualCommand(ConsoleView view, ELocKey name, ELocKey description, bool isOption = false) 
			: base(view.Console, name, description, isOption)
		{
			m_View = view;
		}
	}
}
