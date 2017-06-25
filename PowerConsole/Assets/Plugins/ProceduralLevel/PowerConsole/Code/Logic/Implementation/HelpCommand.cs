namespace ProceduralLevel.PowerConsole.Logic
{
	public class HelpCommand: AConsoleCommand
	{
		public HelpCommand(ConsoleInstance console, string name, string description) : base(console, name, description)
		{
		}

		public Message Command(int someVal, EHelpCategory category = EHelpCategory.Input)
		{
			return new Message(EMessageType.Info, category.ToString());
		}
	}
}
