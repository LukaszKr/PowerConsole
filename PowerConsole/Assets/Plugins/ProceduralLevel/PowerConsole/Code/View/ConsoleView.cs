using ProceduralLevel.PowerConsole.Logic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleView: MonoBehaviour
	{
		public ConsoleInputPanel Input { get; private set; }
		public ConsoleMessagesPanel Messages { get; private set; }
		public ConsoleStyles Styles { get; private set; }
		public ConsoleInstance Console { get; private set; }

		public float Height = 400;

		private bool m_Setup;

		public void Awake()
		{
			Styles = new ConsoleStyles();
			Console = new ConsoleInstance(new DefaultLocalization());

			Input = new ConsoleInputPanel(this);
			Messages = new ConsoleMessagesPanel(this);

			m_Setup = true;

			Messages.AddMessage(new Message(EMessageType.Info, "PowerConsole by Procedural Level"));
			Messages.AddMessage(new Message(EMessageType.Info, "Very long message has to go here, to test if multiline is properly supported." +
				"Event thou this might be a tricky task to do, it has to be supported. Lorem ipsum could also be here, but come on, let's be creative!"));
			Messages.AddMessage(new Message(EMessageType.Info, "Hello World!"));
			for(int x = 0; x < 30; x++)
			{
				Messages.AddMessage(new Message(EMessageType.Warning, x.ToString()));
			}
		}

		public void OnGUI()
		{
			if(m_Setup)
			{
				//hs to be done in OnGUI
				Styles.TryInitialize(false);
				float inputHeight = 25;
				Rect messagesRect = new Rect(0, 0, Screen.width, Height-inputHeight);
				Rect inputRect = new Rect(0, messagesRect.height, Screen.width, inputHeight);
				Messages.Render(messagesRect);
				Input.Render(inputRect);
			}
		}
	}
}
