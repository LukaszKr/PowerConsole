using System;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ChainLabelEntry
	{
		public readonly GUIContent Content;
		public readonly GUIStyle Style;
		public Rect Rect;
		public bool IsDirty = true;

		private int m_SpaceSize = 0;

		public ChainLabelEntry(GUIContent content, GUIStyle style)
		{
			Content = content;
			Style = style;

			CharacterInfo character;
			Font font = style.font;
			if(font == null)
			{
				font = GUI.skin.font;
			}
			font.GetCharacterInfo(' ', out character);
			m_SpaceSize = character.glyphWidth;
		}

		public void Render(Rect rect, ChainLabelEntry prev)
		{
			TryUpdateSize(rect, prev);

			GUI.Label(Rect, Content, Style);
		}

		private void TryUpdateSize(Rect rect, ChainLabelEntry prev)
		{
			if(!IsDirty)
			{
				return;
			}
			
			IsDirty = false;
			Vector2 size = Style.CalcSize(Content);
			int spaces = CountSpaces(Content.text);
			if(spaces > 0)
			{
				size.Set(size.x+spaces*m_SpaceSize, size.y);
			}

			if(prev != null)
			{
				Rect = new Rect(prev.Rect.x+prev.Rect.width, prev.Rect.y, size.x, size.y);
			}
			else
			{
				Rect = new Rect(rect.x, rect.y, size.x, size.y);
			}
		}

		private int CountSpaces(string text)
		{
			int count = 0;
			for(int x = 0; x < text.Length; x++)
			{
				if(text[x] == ' ')
				{
					count ++;
				}
				else
				{
					break;
				}
			}

			for(int x = text.Length-1; x >= 0; x--)
			{
				if(text[x] == ' ')
				{
					count ++;
				}
				else
				{
					break;
				}
			}

			return Math.Min(text.Length, count);
		}

		public override string ToString()
		{
			return string.Format("[Content: {0}]", Content.text);
		}
	}
}
