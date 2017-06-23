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
		public ConsoleInput UserInput { get; private set; }

		public float Height = 400;
		public bool DisplayAuthor = true;

		public TextAsset LocalizationCSV;

		public void Awake()
		{
			Styles = new ConsoleStyles();
			Console = new ConsoleInstance(new LocalizationManager());

			UserInput = new ConsoleInput();
			Input = new ConsoleInputPanel(this);
			Messages = new ConsoleMessagesPanel(this);

			if(DisplayAuthor)
			{
				Messages.AddMessage(new Message(EMessageType.Info, "PowerConsole by Procedural Level"));
			}

		}

		public void OnGUI()
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
