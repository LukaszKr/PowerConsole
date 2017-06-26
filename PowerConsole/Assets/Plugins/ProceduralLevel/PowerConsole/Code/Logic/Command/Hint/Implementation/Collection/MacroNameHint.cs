using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class MacroNameHint: ACollectionHint<string>
	{
		private MacroState m_MacroState;
		private List<Macro> m_Macros;

		public MacroNameHint(MacroState state, List<Macro> macros)
		{
			m_MacroState = state;
			m_Macros = macros;
		}

		protected override string[] GetAllOptions()
		{
			if(m_MacroState.IsRecording)
			{
				return new string[] { };
			}
			else
			{
				string[] names = new string[m_Macros.Count];
				for(int x = 0; x < m_Macros.Count; x++)
				{
					names[x] = m_Macros[x].Name;
				}
				return names;
			}
		}
	}
}
