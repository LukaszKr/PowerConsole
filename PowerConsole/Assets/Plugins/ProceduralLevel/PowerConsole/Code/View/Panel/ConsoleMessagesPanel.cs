using ProceduralLevel.PowerConsole.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public  class ConsoleMessagesPanel: AConsolePanel
	{
		private float m_ScrollPosition = 0;

		private List<MessageView> m_MessageBuffer = new List<MessageView>();

		private Rect m_ScrollRect;
		private Rect m_ViewRect;

		public ConsoleMessagesPanel(ConsoleView consoleView) : base(consoleView)
		{
		}

		public void AddMessage(Message message)
		{
			m_MessageBuffer.Add(new MessageView(message));
		}

		protected override void OnRender(Rect rect)
		{
			m_ViewRect = new Rect(rect.x, rect.y, rect.width-Styles.ScrollbarWidth, TotalHeight());

			m_ScrollPosition = GUI.VerticalScrollbar(m_ScrollRect, m_ScrollPosition, rect.height, 0, m_ViewRect.height);
			
			float hOffset = 0;
			float minOffset = m_ScrollPosition;
			float maxOffset = minOffset+rect.height;
			for(int x = 0; x < m_MessageBuffer.Count; x++)
			{
				MessageView message = m_MessageBuffer[x];
				float newHOffset = hOffset+message.Height;
				if(newHOffset >= minOffset && hOffset < maxOffset)
				{
					Rect renderRect = new Rect(rect.x, rect.y+hOffset-m_ScrollPosition, rect.width, rect.height);
					message.Render(renderRect, Styles);
				}
				hOffset = newHOffset;
			}
		}

		private void HandleInput()
		{
			Event evnt = Event.current;
			if(evnt != null)
			{
				if(evnt.isScrollWheel)
				{
					Debug.Log(evnt.delta);
				}
			}
		}

		private float TotalHeight()
		{
			float height = 0;
			for(int x = 0; x < m_MessageBuffer.Count; x++)
			{
				height += m_MessageBuffer[x].Height;
			}
			return height;
		}

		protected override void OnSizeChanged(Rect rect)
		{
			m_ScrollRect = new Rect(rect.width-Styles.ScrollbarWidth, 0, Styles.ScrollbarWidth, rect.height);

			for(int x = 0; x < m_MessageBuffer.Count; x++)
			{
				m_MessageBuffer[x].MarkAsDirty();
			}
		}
	}
}
