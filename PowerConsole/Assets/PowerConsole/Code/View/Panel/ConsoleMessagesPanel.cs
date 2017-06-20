using ProceduralLevel.PowerConsole.Logic;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralLevel.PowerConsole.View
{
	public  class ConsoleMessagesPanel: AConsolePanel
	{
		public ScrollRect ScrollRect;
		public AConsoleMessageView MessagePrefab;

		private List<AConsoleMessageView> m_Views = new List<AConsoleMessageView>();

		private List<MessageViewDetails> m_MessageBuffer = new List<MessageViewDetails>();


		protected override void OnInitialized()
		{
			m_Console.OnMessage.AddListener(MessageHandler);
		}

		private void MessageHandler(Message message)
		{
			m_MessageBuffer.Add(new MessageViewDetails(message, 20));
			RepositionViews();
		}

		public void RepositionViews()
		{
			RectTransform contentTransform = ScrollRect.content.GetComponent<RectTransform>();

			RectTransform rectTransform = ScrollRect.viewport.GetComponent<RectTransform>();
			float scrollValue = ScrollRect.verticalScrollbar.value;
			float minOffset = contentTransform.rect.y;
			float maxOffset = minOffset+rectTransform.rect.height;
			float hOffset = 0;
			int activeIndex = 0;

			for(int x = 0; x < m_MessageBuffer.Count; x++)
			{
				MessageViewDetails viewDetails = m_MessageBuffer[x];
				if(hOffset >= minOffset && hOffset <= maxOffset)
				{
					AConsoleMessageView view;
					if(activeIndex < m_Views.Count)
					{
						view = m_Views[activeIndex];
						activeIndex++;
					}
					else
					{
						view = CreateMessageView();
					}
					view.SetMessage(viewDetails.Message);
					view.transform.localPosition = new Vector2(0, -hOffset);
				}
				hOffset += viewDetails.Height;
			}

			contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, hOffset);
		}

		private AConsoleMessageView CreateMessageView()
		{
			AConsoleMessageView messageView = Instantiate(MessagePrefab);
			messageView.transform.position = new Vector3(0, 0, 0);
			messageView.transform.SetParent(ScrollRect.content.transform, false);
			m_Views.Add(messageView);
			return messageView;
		}
	}
}
