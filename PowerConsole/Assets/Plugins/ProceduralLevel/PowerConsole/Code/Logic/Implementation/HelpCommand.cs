namespace ProceduralLevel.PowerConsole.Logic
{
	public class HelpCommand: AConsoleCommand
	{
		public HelpCommand(ConsoleInstance console) : base(console, ELocKey.CmdHelpName, ELocKey.CmdHelpDesc)
		{
		}

		public Message Command(EHelpCategory category = EHelpCategory.Input)
		{
			switch(category)
			{
				case EHelpCategory.Input:
					return CreateMessage(EMessageType.Normal, ELocKey.ResHelpInput);
				case EHelpCategory.Macro:
					return CreateMessage(EMessageType.Normal, ELocKey.ResHelpMacro);
				default:
					return CreateMessage(EMessageType.Normal, ELocKey.ResHelpUnknownTopic);
			}
		}
	}
}
