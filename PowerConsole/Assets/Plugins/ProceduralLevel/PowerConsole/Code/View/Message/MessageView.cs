using ProceduralLevel.PowerConsole.Logic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class MessageView
	{
		public readonly Message Message;
		public float Height { get; private set; }
		public bool IsDirty { get; private set; }
		private readonly GUIContent m_Content;

		public MessageView(Message message)
		{
			Message = message;
			m_Content = new GUIContent(message.Value);
			MarkAsDirty();
		}

		public void MarkAsDirty()
		{
			IsDirty = true;
		}

		public void Render(Rect rect, ConsoleStyles style)
		{
			GUIStyle textStyle = style[Message.Type];
			GUI.Label(rect, m_Content, textStyle);
		}

		public void UpdateSize(float maxWidth, GUIStyle textStyle)
		{
			if(IsDirty)
			{
				Height = textStyle.CalcHeight(m_Content, maxWidth);
				IsDirty = false;
			}
		}

		public override string ToString()
		{
			return string.Format("[IsDirty: {0}, Height: {1}, Message: {2}]", IsDirty.ToString(), Height.ToString(), Message.ToString());
		}
	}
}
