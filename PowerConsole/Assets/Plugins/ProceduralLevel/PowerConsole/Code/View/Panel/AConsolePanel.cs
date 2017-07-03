using ProceduralLevel.PowerConsole.Logic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public abstract class AConsolePanel
	{
		protected ConsoleView m_ConsoleView;

		public LocalizationManager Localization { get { return Console.Localization; } }
		public ConsoleInstance Console { get { return m_ConsoleView.Console; } }
		public ConsoleStyles Styles { get { return m_ConsoleView.Styles; } }

		public InputModule Input { get { return Console.InputModule; } }
		public HintModule Hint { get { return Console.HintModule; } }

		protected int m_LastWidth = 0;
		protected int m_LastHeight = 0;
		private bool m_Initialized = false;

		public AConsolePanel(ConsoleView consoleView)
		{
			m_ConsoleView = consoleView;
		}

		public void Render(Rect rect)
		{
			TryInitialize();

			GUI.BeginGroup(rect, Styles.Box);
			UpdateSizes(rect.size);
			OnRender(rect.size);
			GUI.EndGroup();
		}

		private void TryInitialize()
		{
			if(m_Initialized)
			{
				return;
			}
			m_Initialized = true;
			Initialize();
		}

		protected abstract void OnRender(Vector2 size);
		protected abstract void OnSizeChanged(Vector2 size);
		protected virtual void Initialize() { }
		public abstract float PreferedHeight(float availableHeight);

		private void UpdateSizes(Vector2 size)
		{
			int width = (int)size.x;
			int height = (int)size.y;
			if(width != m_LastWidth || height != m_LastHeight)
			{
				m_LastWidth = width;
				m_LastHeight = height;
				OnSizeChanged(size);
			}
		}
	}
}
