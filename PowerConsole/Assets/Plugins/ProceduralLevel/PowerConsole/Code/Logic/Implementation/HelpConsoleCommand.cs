namespace ProceduralLevel.PowerConsole.Logic
{
	public class HelpConsoleCommand: AConsoleCommand
	{
		public HelpConsoleCommand(string name, string description) : base(name, description)
		{
		}

		public Message Command(int someVal, EHelpCategory category = EHelpCategory.Input)
		{
			return new Message(EMessageType.Info, category.ToString());
		}

		public override bool IsValid()
		{
			return true;
		}
	}
}
