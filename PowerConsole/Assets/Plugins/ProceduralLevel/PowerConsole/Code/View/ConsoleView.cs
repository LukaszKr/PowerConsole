﻿using ProceduralLevel.PowerConsole.Logic;
using System;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleView: MonoBehaviour
	{
		public ConsoleDetailsPanel Details { get; private set; }
		public ConsoleInputPanel Input { get; private set; }
		public ConsoleMessagesPanel Messages { get; private set; }
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
			float inputHeight = Styles.InputHeight;
			float offset = 0;

			if(DisplayTitle)
			{
				Rect detailsRect = new Rect(0, offset, Screen.width, inputHeight);
				Details.Render(detailsRect);
				offset += inputHeight;
			}

			Rect messagesRect = new Rect(0, offset, Screen.width, Height-inputHeight*2);
			Messages.Render(messagesRect);
			offset += messagesRect.height;

			Rect inputRect = new Rect(0, offset, Screen.width, inputHeight);
			Input.Render(inputRect);
			offset += inputHeight;
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
			}
		}
	}
}
