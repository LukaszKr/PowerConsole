using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.View
{
	public class ClearCommand: AVisualCommand
	{
		public ClearCommand(ConsoleView view) : base(view, ELocKey.CmdClearName, ELocKey.CmdClearDesc)
		{
		}

		public Message Command()
		{
			m_View.Messages.ClearMessages();
			return null;
		}
	}
}
