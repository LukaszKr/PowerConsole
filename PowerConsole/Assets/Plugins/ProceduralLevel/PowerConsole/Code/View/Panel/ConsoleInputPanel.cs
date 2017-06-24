using ProceduralLevel.PowerConsole.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleInputPanel: AConsolePanel
	{
		public string UserInput { get; private set; }

		private GUIContent m_ButtonText;

		private Rect m_InputRect;
		private Rect m_SubmitRect;

		private List<Exception> m_InputErrors = new List<Exception>();

		public ConsoleInputPanel(ConsoleView consoleView) : base(consoleView)
		{
			UserInput = "";
			m_ButtonText = new GUIContent(Localization.GetLocalizedKey(ELocalizationKey.Input_Submit));
		}

		protected override void OnRender(Vector2 size)
		{
			UserInput = GUI.TextField(m_InputRect, UserInput, Styles.InputText);
			if(GUI.Button(m_SubmitRect, m_ButtonText) || Input.ExecuteCommand())
			{
				Console.Execute(UserInput);
				UserInput = "";
			}
			if(GUI.changed)
			{
				m_InputErrors.Clear();
				List<Query> queries = Console.ParseQuery(UserInput);
				int cursor = TextEditorHelper.GetCursor();
				Argument arg = null;
				Query query = null;
				for(int x = 0; x < queries.Count; x++)
				{
					query = queries[x];
					arg = query.GetArgumentAt(cursor);
					if(arg != null)
					{
						break;
					}
				}
				if(query != null)
				{
					AConsoleCommand command = Console.FindCommand(query.Name.Value);
					if(command != null)
					{
						try
						{
							command.Method.MapArguments(query);
						}
						catch(Exception e)
						{
							m_InputErrors.Add(e);
						}
						try
						{
							Console.ParseValues(query);
						}
						catch(Exception e)
						{
							m_InputErrors.Add(e);
						}
					}
				}

				Debug.Log(arg);
				for(int x = 0; x < m_InputErrors.Count; x++)
				{
					Debug.LogError(m_InputErrors[x].ToString());
				}
			}
		}

		protected override void OnSizeChanged(Vector2 size)
		{
			float inputWidth = size.x*0.8f;
			m_InputRect = new Rect(4, 0, inputWidth, size.y);
			m_SubmitRect = new Rect(inputWidth, 0, size.x-inputWidth, size.y);
		}
	}
}
