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
		public ConsoleIssuePanel Issues { get; private set; }

		public ConsoleStyles Styles { get; private set; }
		public ConsoleInstance Console { get; private set; }
		public ConsoleInput UserInput { get; private set; }

		public float Height = 400;

		public bool PrintExecutedCommand = true;

		public bool DisplayTitle = true;
		public bool DisplayHints = true;
		public bool DisplayIssues = true;
		public bool InitializeDefaults = true;
		public bool Active = false;

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
			UserInput.Update();

			if(!Active)
			{
				return;
			}

			//has to be done in OnGUI
			Styles.TryInitialize(false);
			float offset = 0;

			float hDetails = GetHeight(Details, DisplayTitle);
			float hMessages = GetHeight(Messages);
			float hInput = GetHeight(Input);
			float hHints = GetHeight(Hints, DisplayHints);
			float hIssues = GetHeight(Issues, DisplayIssues);

			hMessages = hMessages-hDetails-hInput;

			Rect detailsRect = CreateRect(offset, hDetails);
			offset += hDetails;

			Rect messagesRect = CreateRect(offset, hMessages);
			offset += hMessages;

			Rect inputRect = CreateRect(offset, hInput);
			offset += hInput;

			Rect hintRect = CreateRect(offset, hHints);
			offset += hHints;

			Rect issuesRect = CreateRect(offset, hIssues);
			offset += hIssues;

			if(DisplayTitle)
			{
				Details.Render(detailsRect);
			}

			Console.PrintExecutedCommand = PrintExecutedCommand;

			Messages.Render(messagesRect);
			Input.Render(inputRect);
			if(DisplayHints && hHints > 1)
			{
				Hints.Render(hintRect);
			}
			if(DisplayIssues && hIssues > 1)
			{
				Issues.Render(issuesRect);
			}
		}

		private void TryInitialize()
		{
			if(!m_Initialized)
			{
				m_Initialized = true;
				LocalizationManager localization = new LocalizationManager();
				localization.Load("en-us", LocalizationCSV.text);

				Styles = new ConsoleStyles();
				Console = new ConsoleInstance(localization, new UnityPersistence());
				if(InitializeDefaults)
				{
					Console.SetupDefault();
				}
				UserInput = new ConsoleInput(this);
				
				Details = new ConsoleDetailsPanel(this);
				Messages = new ConsoleMessagesPanel(this);
				Hints = new ConsoleHintPanel(this);
				Input = new ConsoleInputPanel(this);
				Issues = new ConsoleIssuePanel(this);

				Console.AddCommand(new ClearCommand(this));
			}
		}

		private float GetHeight(AConsolePanel panel, bool display = true)
		{
			return (display? panel.PreferedHeight(Height) : 0);
		}

		private Rect CreateRect(float offset, float height)
		{
			return new Rect(0, offset, Screen.width, height).AddMargin(H_MARGIN, V_MARGIN);
		}

		public void ToggleActive()
		{
			Active = !Active;
			if(Active)
			{
				Input.StealFocus();
			}
		}
	}
}
