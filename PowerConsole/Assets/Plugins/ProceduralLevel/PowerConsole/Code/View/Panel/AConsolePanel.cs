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
			GUI.BeginGroup(rect, Styles.Box);
			UpdateSizes(rect.size);
			OnRender(rect.size);
			GUI.EndGroup();
		}

		protected abstract void OnRender(Vector2 size);
		protected abstract void OnSizeChanged(Vector2 size);

		private void UpdateSizes(Vector2 size)
		{
			int width = (int)size.x;
			if(width != m_LastWidth)
			{
				m_LastWidth = width;
				OnSizeChanged(size);
			}
		}
	}
}
