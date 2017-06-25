namespace ProceduralLevel.PowerConsole.Logic
{
	public class PrintCommand: AConsoleCommand
	{
		public PrintCommand(ConsoleInstance console, string name, string description) : base(console, name, description)
		{
		}

		public Message Command(string str, EMessageType messageType = EMessageType.Normal)
		{
			return new Message(messageType, str);
		}
	}
}
