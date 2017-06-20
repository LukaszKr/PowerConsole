using ProceduralLevel.PowerConsole.Logic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public abstract class AConsolePanel: MonoBehaviour
	{
		protected ConsoleInstance m_Console;

		public void Initialize(ConsoleInstance console)
		{
			m_Console = console;
			OnInitialized();
		}

		protected abstract void OnInitialized();
	}
}
