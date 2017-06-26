namespace ProceduralLevel.PowerConsole.Logic
{
	public class ClearHistoryCommand: AConsoleCommand
	{
		public ClearHistoryCommand(ConsoleInstance console) : base(console, ELocKey.CmdClearHistoryName, ELocKey.CmdClearHistoryDesc)
		{
		}

		public Message Command()
		{
			int count = Console.HistoryState.Count;
			Console.HistoryState.ClearExecutionHistory();
			return new Message(EMessageType.Success, Localization.Get(ELocKey.ResClearHistory, count));
		}
	}
}
