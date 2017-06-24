using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleDetailsPanel: AConsolePanel
	{
		private const string TITLE = "PowerConsole by Procedural Level";

		private Rect m_DisplayRect;
		private GUIContent m_Title;

		public ConsoleDetailsPanel(ConsoleView consoleView) : base(consoleView)
		{
			m_Title = new GUIContent(TITLE);
		}

		public override float PreferedHeight(float availableHeight)
		{
			return Styles.InputHeight;
		}

		protected override void OnRender(Vector2 size)
		{
			GUI.Label(m_DisplayRect, m_Title, Styles.TitleText);
		}

		protected override void OnSizeChanged(Vector2 size)
		{
			m_DisplayRect = new Rect(0, 0, size.x, size.y);
		}
	}
}
