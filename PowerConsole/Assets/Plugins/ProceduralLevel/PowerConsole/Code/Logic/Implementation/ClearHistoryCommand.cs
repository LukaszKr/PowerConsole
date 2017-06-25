namespace ProceduralLevel.PowerConsole.Logic
{
	public class ClearHistoryCommand: AConsoleCommand
	{
		public ClearHistoryCommand(ConsoleInstance console, string name, string description) : base(console, name, description)
		{
		}

		public Message Command()
		{
			int count = Console.HistoryState.Count;
			Console.HistoryState.ClearExecutionHistory();
			return new Message(EMessageType.Success, string.Format("Removed {0} entries from execution history", count));
		}

		public override bool IsValid()
		{
			return true;
		}
	}
}
