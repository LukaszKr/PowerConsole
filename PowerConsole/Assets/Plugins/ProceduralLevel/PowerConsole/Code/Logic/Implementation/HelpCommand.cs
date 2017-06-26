namespace ProceduralLevel.PowerConsole.Logic
{
	public class HelpCommand: AConsoleCommand
	{
		public HelpCommand(ConsoleInstance console) : base(console, ELocKey.CmdHelpName, ELocKey.CmdHelpDesc)
		{
		}

		public Message Command(EHelpCategory category = EHelpCategory.Input)
		{
			return new Message(EMessageType.Info, category.ToString());
		}
	}
}
