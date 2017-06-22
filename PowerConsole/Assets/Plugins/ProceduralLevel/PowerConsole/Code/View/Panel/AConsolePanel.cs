using ProceduralLevel.PowerConsole.Logic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public abstract class AConsolePanel
	{
		protected ConsoleView m_ConsoleView;

		public ALocalization Localization { get { return Console.Localization; } }
		public ConsoleInstance Console { get { return m_ConsoleView.Console; } }
		public ConsoleStyles Styles { get { return m_ConsoleView.Styles; } }

		protected int m_LastWidth = 0;

		public AConsolePanel(ConsoleView consoleView)
		{
			m_ConsoleView = consoleView;
		}

		public void Render(Rect rect)
		{
			GUI.Box(rect, string.Empty);
			UpdateSizes(rect);
			OnRender(rect);
		}

		protected abstract void OnRender(Rect rect);
		protected abstract void OnSizeChanged(Rect rect);

		private void UpdateSizes(Rect rect)
		{
			int width = (int)rect.width;
			if(width != m_LastWidth)
			{
				m_LastWidth = width;
				OnSizeChanged(rect);
			}
		}
	}
}
