﻿using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public static class TextEditorHelper
	{
		public static TextEditor GetEditor()
		{
			TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
			return editor;
		}

		public static int GetCursor()
		{
			return GetEditor().cursorIndex;
		}
	}
}