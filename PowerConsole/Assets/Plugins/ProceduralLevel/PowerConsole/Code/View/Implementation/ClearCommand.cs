using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.View
{
	public class ClearCommand: AVisualCommand
	{
		public ClearCommand(ConsoleView view, string name, string description) : base(view, name, description)
		{
		}

		public Message Command()
		{
			m_View.Messages.ClearMessages();
			return null;
		}
	}
}
