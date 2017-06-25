using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public static class StyleFactory
	{
		public static GUIStyle TextStyle(float redOffset, float greenOffset, float blueOffset, float alpha,
			FontStyle fontStyle = FontStyle.Normal, TextAnchor alignment = TextAnchor.UpperLeft)
		{
			GUIStyle style = new GUIStyle("label");
			style.fontStyle = fontStyle;
			Color dColor = style.normal.textColor;
			style.normal.textColor = new Color(dColor.r+redOffset, dColor.g+greenOffset, dColor.b+blueOffset, alpha);
			style.alignment = alignment;
			return style;
		}
	}
}
