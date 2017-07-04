using ProceduralLevel.PowerConsole.Logic;
using ProceduralLevel.UnityCommon.Ext;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleInputPanel: AConsolePanel
	{
		private const string INPUT_NAME = "Input";
		private const int PADDING = 6;
		private const int SUBMIT_MARGIN = 2;

		private GUIContent m_ButtonText;

		private Rect m_InputRect;
		private Rect m_SubmitRect;

		private int m_DesiredCursor = -1;
		private bool m_StealFocus;


		public ConsoleInputPanel(ConsoleView consoleView) : base(consoleView)
		{
			m_ButtonText = new GUIContent(Localization.Get(ELocKey.UIInputSubmit));

			Console.InputModule.OnCursorMoved.AddListener(CursorMovedHandler);
		}

		public override float PreferedHeight(float availableHeight)
		{
			return Styles.InputHeight;
		}

		protected override void OnRender(Vector2 size)
		{
			bool isRepaint = (Event.current != null && Event.current.type == EventType.Repaint);

			GUI.SetNextControlName(INPUT_NAME);
			if(m_DesiredCursor >= 0)
			{
				TextEditorHelper.SetText(Console.InputModule.CurrentInput);
				TextEditorHelper.SetCursor(m_DesiredCursor);
				m_DesiredCursor = -1;
			}

			string newInput = GUI.TextField(m_InputRect, Console.InputModule.CurrentInput, Styles.InputText);
			if(isRepaint)
			{
				if(m_StealFocus)
				{
					m_StealFocus = false;
					GUI.FocusControl(INPUT_NAME);
				}
			}

			if(GUI.Button(m_SubmitRect, m_ButtonText))
			{
				Console.InputModule.Execute();
			}
			int newCursor = TextEditorHelper.GetCursor();
			if(m_DesiredCursor >= 0)
			{
				newCursor = m_DesiredCursor;
			}

			if(GUI.changed)
			{
				if(!Console.HintModule.IteratingHints || Console.HintModule.Current.Hint.Length == 0)
				{
					Console.InputModule.SetInput(newInput, newCursor);
				}
				else if(m_DesiredCursor < 0)
				{
					//if user deleted
					if(newCursor < Console.InputModule.Cursor)
					{
						Console.HintModule.CancelHint();
					}
					else if(newCursor > Console.InputModule.Cursor)
					{
						Console.InputModule.SetInput(newInput, newCursor);
					}
				}
			}
			else
			{
				Console.InputModule.SetCursor(newCursor);
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

		public void StealFocus()
		{
			m_StealFocus = true;
		}
	}
}
