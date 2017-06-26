using System.Text;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class MacroManagerCommand: AConsoleCommand
	{
		public override bool ObeyLock { get { return false; } }

		public MacroManagerCommand(ConsoleInstance console) : base(console, ELocKey.CmdMacroName, ELocKey.CmdMacroDesc)
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
						return new Message(EMessageType.Error, Localization.Get(ELocKey.ResMacroNameEmpty));
					}
					if(Console.MacroState.IsRecording)
					{
						return new Message(EMessageType.Error, Localization.Get(ELocKey.ResMacroAlreadyRecording));
					}
					Console.MacroState.StartRecording(name);
					return new Message(EMessageType.Success, Localization.Get(ELocKey.ResMacroRecording));
				case EMacroMode.List:
					StringBuilder sb = new StringBuilder();
					sb.Append(Localization.Get(ELocKey.ResMacroList));
					for(int x = 0; x < Console.MacroState.Count; x++)
					{
						Macro macro = Console.MacroState.Get(x);
						sb.AppendLine(macro.Name);
					}
					return new Message(EMessageType.Normal, sb.ToString());
				case EMacroMode.Remove:
					if(Console.MacroState.RemoveMacro(name))
					{
						return new Message(EMessageType.Success, Localization.Get(ELocKey.ResMacroRemoved, name));
					}
					else
					{
						return new Message(EMessageType.Warning, Localization.Get(ELocKey.ResMacroNotRemoved, name));
					}
				case EMacroMode.Save:
					if(!Console.MacroState.IsRecording)
					{
						return new Message(EMessageType.Error, Localization.Get(ELocKey.ResMacroNotRecording));
					}
					Console.MacroState.Save();
					return new Message(EMessageType.Success, Localization.Get(ELocKey.ResMacroSaved));
				default:
					return new Message(EMessageType.Error, Console.Localization.Get(ELocKey.ResMacroModeUnknown, mode));
			}
		}
	}
}
