namespace ProceduralLevel.PowerConsole.Logic
{
	public class PrintCommand: AConsoleCommand
	{
		public PrintCommand(ConsoleInstance console) : base(console, ELocKey.CmdPrintName, ELocKey.CmdPrintDesc)
		{
		}

		public Message Command(string str, EMessageType messageType = EMessageType.Normal)
		{
			return new Message(messageType, str);
		}
	}
}
