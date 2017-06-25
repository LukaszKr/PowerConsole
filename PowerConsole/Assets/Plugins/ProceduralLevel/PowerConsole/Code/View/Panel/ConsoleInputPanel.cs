using ProceduralLevel.PowerConsole.Logic;
using ProceduralLevel.UnityCommon.Ext;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleInputPanel: AConsolePanel
	{
		private const int PADDING = 6;
		private const int SUBMIT_MARGIN = 2;

		private GUIContent m_ButtonText;

		private Rect m_InputRect;
		private Rect m_SubmitRect;

		private int m_DesiredCursor = -1;

		public ConsoleInputPanel(ConsoleView consoleView) : base(consoleView)
		{
			m_ButtonText = new GUIContent(Localization.GetLocalizedKey(ELocalizationKey.Input_Submit));

			Console.InputState.OnCursorMoved.AddListener(CursorMovedHandler);
		}

		public override float PreferedHeight(float availableHeight)
		{
			return Styles.InputHeight;
		}

		protected override void OnRender(Vector2 size)
		{
			bool isRepaint = (Event.current != null && Event.current.type == EventType.Repaint);

			string newInput = GUI.TextField(m_InputRect, Console.InputState.CurrentInput, Styles.InputText);

			if(isRepaint)
			{
				if(m_DesiredCursor >= 0)
				{
					TextEditorHelper.SetCursor(m_DesiredCursor);
					m_DesiredCursor = -1;
				}
			}

			if(GUI.Button(m_SubmitRect, m_ButtonText))
			{
				Console.InputState.Execute();
			}
			int newCursor = TextEditorHelper.GetCursor();
			if(m_DesiredCursor >= 0)
			{
				newCursor = m_DesiredCursor;
			}
			if(!Console.HintState.IteratingHints)
			{
				Console.InputState.SetInput(newInput, newCursor);
			}
			else if(m_DesiredCursor < 0)
			{
				if(newCursor < Console.InputState.Cursor)
				{
					Console.HintState.CancelHint();
				}
				else if(newCursor > Console.InputState.Cursor)
				{
					Console.InputState.SetInput(newInput, newCursor);
				}
			}
		}

		protected override void OnSizeChanged(Vector2 size)
		{
			float inputWidth = size.x*0.8f;
			m_InputRect = new Rect(PADDING, 0, inputWidth-PADDING*2, size.y);
			m_SubmitRect = new Rect(inputWidth, 0, size.x-inputWidth, size.y).AddMargin(SUBMIT_MARGIN, SUBMIT_MARGIN);
		}

		private void CursorMovedHandler(int cursor)
		{
			m_DesiredCursor = cursor;
		}
	}
}
