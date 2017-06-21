using ProceduralLevel.PowerConsole.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public  class ConsoleMessagesPanel: AConsolePanel
	{
		private Vector2 m_ScrollPosition;

		private List<MessageView> m_MessageBuffer = new List<MessageView>();

		public ConsoleMessagesPanel(ConsoleInstance console) : base(console)
		{
			m_Console.OnMessage.AddListener(MessageHandler);
		}

		private void MessageHandler(Message message)
		{
			m_MessageBuffer.Add(new MessageView(message, 0));
			RepositionViews();
		}

		public void RepositionViews()
		{

		}

	}
}
