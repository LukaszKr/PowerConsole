namespace ProceduralLevel.PowerConsole.Logic
{
	public class ClearConsoleCommand: AConsoleCommand
	{
		public ClearConsoleCommand(string name, string description) : base(name, description)
		{
		}

		public Message Command()
		{
			return new Message(EMessageType.Error, "Not Implemented");
		}

		public override bool IsValid()
		{
			return true;
		}
	}
}
