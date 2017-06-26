namespace ProceduralLevel.PowerConsole.Logic
{
	public class MacroManagerCommand: AConsoleCommand
	{
		public override bool ObeyLock { get { return false; } }

		public MacroManagerCommand(ConsoleInstance console, string name, string description) : base(console, name, description)
		{
		}

		public override AHint GetHintFor(HintManager manager, int parameterIndex)
		{
			if(parameterIndex == 1)
			{
				return Console.MacroState.NameHint;
			}
			return base.GetHintFor(manager, parameterIndex);
		}

		public Message Command(EMacroMode mode, string name = "")
		{
			switch(mode)
			{
				case EMacroMode.Record:
					if(string.IsNullOrEmpty(name))
					{
						return new Message(EMessageType.Error, Localization.Get(ELocKey.CmdMacroNameEmpty));
					}
					if(Console.MacroState.IsRecording)
					{
						return new Message(EMessageType.Error, Localization.Get(ELocKey.CmdMacroAlreadyRecording));
					}
					Console.MacroState.StartRecording(name);
					return new Message(EMessageType.Info, Localization.Get(ELocKey.CmdMacroRecording));
				case EMacroMode.List:
					return null;
				case EMacroMode.Remove:
					Console.MacroState.RemoveMacro(name);
					return null;
				case EMacroMode.Save:
					Console.MacroState.Save();
					return null;
				default:
					return new Message(EMessageType.Error, Console.Localization.MacroModeNotSupported(mode));
			}
		}
	}
}
