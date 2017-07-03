namespace ProceduralLevel.PowerConsole.Logic
{
	public class ClearHistoryCommand: AConsoleCommand
	{
		public ClearHistoryCommand(ConsoleInstance console) : base(console, ELocKey.CmdClearHistoryName, ELocKey.CmdClearHistoryDesc)
		{
		}

		public Message Command()
		{
			int count = Console.HistoryModule.Count;
			Console.HistoryModule.ClearExecutionHistory();
			return CreateMessage(EMessageType.Success, ELocKey.ResClearHistory, count);
		}
	}
}
