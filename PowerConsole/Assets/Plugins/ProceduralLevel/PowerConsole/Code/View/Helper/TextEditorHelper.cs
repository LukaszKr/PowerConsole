using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public static class TextEditorHelper
	{
		public static TextEditor GetEditor()
		{
			TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
			return editor;
		}

		public static void SetText(string text)
		{
			GetEditor().text = text;
		}

		public static int GetCursor()
		{
			return GetEditor().cursorIndex;
		}

		public static void SetCursor(int position)
		{
			TextEditor editor = GetEditor();
			editor.cursorIndex = position;
			editor.selectIndex = position;
		}
	}
}
