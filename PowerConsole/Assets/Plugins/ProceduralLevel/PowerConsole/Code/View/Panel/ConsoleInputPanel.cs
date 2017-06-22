using ProceduralLevel.PowerConsole.Logic;
using ProceduralLevel.UnityCommon.Ext;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleInputPanel: AConsolePanel
	{
		public string UserInput { get; private set; }

		private GUIContent m_ButtonText;

		private Rect[] m_Rects;

		public ConsoleInputPanel(ConsoleView consoleView) : base(consoleView)
		{
			UserInput = "";
			m_ButtonText = new GUIContent(Localization.GetLocalizedKey(ELocalizationKey.Input_Submit));
		}

		protected override void OnRender(Rect rect)
		{
			UserInput = GUI.TextField(m_Rects[0], UserInput);
			if(GUI.Button(m_Rects[1], m_ButtonText))
			{

			}
		}

		protected override void OnSizeChanged(Rect rect)
		{
			m_Rects = rect.SplitHorizontal(0.8f, 0.2f);
		}
	}
}
