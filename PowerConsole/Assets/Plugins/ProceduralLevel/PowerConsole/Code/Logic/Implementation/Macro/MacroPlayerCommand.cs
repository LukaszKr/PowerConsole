using System.Collections;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class MacroPlayerCommand: AConsoleCommand
	{
		private Macro m_Macro;

		public MacroPlayerCommand(ConsoleInstance console, Macro macro) : base(console, macro.Name, console.Localization.Get(ELocKey.CmdMacroPlayerDesc))
		{
			m_Macro = macro;
		}

		public Message Command(float delay = -1)
		{
			if (delay < 0) MacroPlayerHelper.PlayMacro(Console, m_Macro);
			else MacroPlayerHelper.Instance.PlayMacroWithDelay(Console, m_Macro, delay);
			return null;
		}
	}
}
