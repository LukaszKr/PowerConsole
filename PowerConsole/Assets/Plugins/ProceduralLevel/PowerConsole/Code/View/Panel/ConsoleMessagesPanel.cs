﻿using ProceduralLevel.PowerConsole.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public  class ConsoleMessagesPanel: AConsolePanel
	{
		private const float SCROLL_POWER = 10;

		private float m_ScrollPosition = 0;
		private Rect m_ScrollRect;

		private float m_TotalHeight;

		private List<MessageView> m_MessageBuffer = new List<MessageView>();

		public ConsoleMessagesPanel(ConsoleView consoleView) : base(consoleView)
		{
			Console.OnMessage.AddListener(AddMessage);
		}

		public void AddMessage(Message message)
		{
			m_MessageBuffer.Add(new MessageView(message));
			m_ScrollPosition = m_TotalHeight+100;
		}

		protected override void OnRender(Vector2 size)
		{
			for(int x = 0; x < m_MessageBuffer.Count; x++)
			{
				MessageView messagaView = m_MessageBuffer[x];
				messagaView.UpdateSize(size.x, Styles[messagaView.Message.Type]);
			}

			m_TotalHeight = TotalHeight();
			HandleMouse(size);

			m_ScrollPosition = GUI.VerticalScrollbar(m_ScrollRect, m_ScrollPosition, size.y, 0f, m_TotalHeight);

			
			float hOffset = 0;
			float minOffset = m_ScrollPosition;
			float maxOffset = minOffset+size.y;
			for(int x = 0; x < m_MessageBuffer.Count; x++)
			{
				MessageView messageView = m_MessageBuffer[x];
				float newHOffset = hOffset+messageView.Height;
				if(newHOffset >= minOffset && hOffset < maxOffset)
				{
					Rect renderRect = new Rect(4f, hOffset-m_ScrollPosition, size.x-8f, size.y);
					messageView.Render(renderRect, Styles);
				}
				hOffset = newHOffset;
			}
		}

		private void HandleMouse(Vector2 size)
		{
			Event evnt = Event.current;
			if(evnt != null)
			{
				if(evnt.isScrollWheel)
				{
					Vector2 mousePosition = evnt.mousePosition;
					if(mousePosition.x >= 0 && mousePosition.y >= 0 && mousePosition.x <= size.x && mousePosition.y <= size.y)
					{
						m_ScrollPosition += evnt.delta.y*SCROLL_POWER;
					}
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

		protected override void OnSizeChanged(Vector2 size)
		{
			m_ScrollRect = new Rect(size.x-Styles.ScrollbarWidth, 0, Styles.ScrollbarWidth, size.y);

			for(int x = 0; x < m_MessageBuffer.Count; x++)
			{
				m_MessageBuffer[x].MarkAsDirty();
			}
		}
	}
}