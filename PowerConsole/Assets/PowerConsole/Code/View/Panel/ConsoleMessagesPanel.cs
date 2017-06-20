using ProceduralLevel.PowerConsole.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public  class ConsoleMessagesPanel: AConsolePanel
	{
		public GameObject ContentContainer;
		public AConsoleMessageView MessagePrefab;

		private List<AConsoleMessageView> m_ActiveViews = new List<AConsoleMessageView>();

		protected override void OnInitialized()
		{
			m_Console.OnMessage.AddListener(MessageHandler);
		}

		private void MessageHandler(Message message)
		{
			AConsoleMessageView messageView = GetMessage();
			messageView.SetMessage(message);
			m_ActiveViews.Add(messageView);
			RepositionViews();
		}

		private void RepositionViews()
		{
			float hOffset = 0;
			for(int x = 0; x < m_ActiveViews.Count; x++)
			{
				AConsoleMessageView view = m_ActiveViews[x];
				view.transform.localPosition = new Vector2(0, hOffset);
				hOffset -= view.Height;
			}
		}

		private AConsoleMessageView GetMessage()
		{
			AConsoleMessageView messageView = Instantiate(MessagePrefab);
			messageView.transform.position = new Vector3(0, 0, 0);
			messageView.transform.SetParent(ContentContainer.transform, false);
			return messageView;
		}
	}
}
