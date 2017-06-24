using ProceduralLevel.PowerConsole.Logic;
using ProceduralLevel.UnityCommon.Ext;
using System;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleView: MonoBehaviour
	{
		private const int H_MARGIN = 2;
		private const int V_MARGIN = 2;

		public ConsoleDetailsPanel Details { get; private set; }
		public ConsoleInputPanel Input { get; private set; }
		public ConsoleMessagesPanel Messages { get; private set; }
		public ConsoleHintPanel Hints { get; private set; }

		public ConsoleStyles Styles { get; private set; }
		public ConsoleInstance Console { get; private set; }
		public ConsoleInput UserInput { get; private set; }

		public float Height = 400;
		public bool DisplayTitle = true;

		public TextAsset LocalizationCSV;

		[NonSerialized]
		private bool m_Initialized;

		public void Awake()
		{
			TryInitialize();
		}

		public void OnGUI()
		{
			//after recompiling, restart the console
			TryInitialize();

			//hs to be done in OnGUI
			Styles.TryInitialize(false);
			float offset = 0;

			float hDetails = (DisplayTitle? Details.PreferedHeight(Height) : 0);
			float hMessages = Messages.PreferedHeight(Height);
			float hInput = Input.PreferedHeight(Height);
			float hHints = Hints.PreferedHeight(Height);

			hMessages = hMessages-hDetails-hInput-hHints;

			Rect detailsRect = new Rect(0, offset, Screen.width, hDetails).AddMargin(H_MARGIN, V_MARGIN);
			offset += hDetails;

			Rect messagesRect = new Rect(0, offset, Screen.width, hMessages).AddMargin(H_MARGIN, V_MARGIN);
			offset += hMessages;

			Rect inputRect = new Rect(0, offset, Screen.width, hInput).AddMargin(H_MARGIN, V_MARGIN);
			offset += hInput;

			Rect hintRect = new Rect(0, offset, Screen.width, hHints).AddMargin(H_MARGIN, V_MARGIN);
			offset += hHints;

			if(DisplayTitle)
			{
				Details.Render(detailsRect);
			}
			Messages.Render(messagesRect);
			Input.Render(inputRect);
			Hints.Render(hintRect);
		}

		private void TryInitialize()
		{
			if(!m_Initialized)
			{
				m_Initialized = true;

				Styles = new ConsoleStyles();
				Console = new ConsoleInstance(new LocalizationManager());
				UserInput = new ConsoleInput();
				
				Details = new ConsoleDetailsPanel(this);
				Input = new ConsoleInputPanel(this);
				Messages = new ConsoleMessagesPanel(this);
				Hints = new ConsoleHintPanel(this);
			}
		}
	}
}
