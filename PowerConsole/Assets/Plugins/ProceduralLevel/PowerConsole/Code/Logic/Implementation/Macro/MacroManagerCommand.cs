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
				return Console.MacroModule.NameHint;
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
						return CreateMessage(EMessageType.Error, ELocKey.ResMacroNameEmpty);
					}
					if(Console.MacroModule.IsRecording)
					{
						return CreateMessage(EMessageType.Error, ELocKey.ResMacroAlreadyRecording);
					}
					Console.MacroModule.StartRecording(name);
					return CreateMessage(EMessageType.Success, ELocKey.ResMacroRecording);
				case EMacroMode.List:
					StringBuilder sb = new StringBuilder();
					sb.Append(ELocKey.ResMacroList);
					for(int x = 0; x < Console.MacroModule.Count; x++)
					{
						Macro macro = Console.MacroModule.Get(x);
						sb.AppendLine(macro.Name);
					}
					return new Message(EMessageType.Normal, sb.ToString());
				case EMacroMode.Remove:
					if(Console.MacroModule.RemoveMacro(name))
					{
						return CreateMessage(EMessageType.Success, ELocKey.ResMacroRemoved, name);
					}
					else
					{
						return CreateMessage(EMessageType.Warning, ELocKey.ResMacroNotRemoved, name);
					}
				case EMacroMode.Save:
					if(!Console.MacroModule.IsRecording)
					{
						return CreateMessage(EMessageType.Error, ELocKey.ResMacroNotRecording);
					}
					Console.MacroModule.Save();
					return CreateMessage(EMessageType.Success, ELocKey.ResMacroSaved);
				default:
					return CreateMessage(EMessageType.Error, ELocKey.ResMacroModeUnknown, mode);
			}
		}
	}
}
