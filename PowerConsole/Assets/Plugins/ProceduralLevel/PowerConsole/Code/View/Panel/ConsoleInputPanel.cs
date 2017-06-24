using ProceduralLevel.PowerConsole.Logic;
using ProceduralLevel.UnityCommon.Ext;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleInputPanel: AConsolePanel
	{
		private const int PADDING = 6;
		private const int SUBMIT_MARGIN = 2;

		public string UserInput { get; private set; }

		private GUIContent m_ButtonText;

		private Rect m_InputRect;
		private Rect m_SubmitRect;

		private List<Exception> m_InputErrors = new List<Exception>();

		private int m_Cursor;

		public ConsoleInputPanel(ConsoleView consoleView) : base(consoleView)
		{
			UserInput = "";
			m_ButtonText = new GUIContent(Localization.GetLocalizedKey(ELocalizationKey.Input_Submit));
			UpdateInputText();
		}

		public override float PreferedHeight(float availableHeight)
		{
			return Styles.InputHeight;
		}

		protected override void OnRender(Vector2 size)
		{
			UserInput = GUI.TextField(m_InputRect, UserInput, Styles.InputText);
			if(GUI.Button(m_SubmitRect, m_ButtonText))
			{
				Execute();
			}
			int newCursor = TextEditorHelper.GetCursor();
			if(newCursor != m_Cursor)
			{
				m_Cursor = newCursor;
				m_InputErrors.Clear();
				UpdateInputText();
			}
		}

		private void UpdateInputText()
		{
			List<Query> queries = Console.ParseQuery(UserInput);
			int cursor = TextEditorHelper.GetCursor();

			AConsoleCommand command = null;
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
				command = Console.FindCommand(query.Name.Value);
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
			m_ConsoleView.Hints.UpdateHint(command, arg);
		}

		protected override void OnSizeChanged(Vector2 size)
		{
			float inputWidth = size.x*0.8f;
			m_InputRect = new Rect(PADDING, 0, inputWidth-PADDING*2, size.y);
			m_SubmitRect = new Rect(inputWidth, 0, size.x-inputWidth, size.y).AddMargin(SUBMIT_MARGIN, SUBMIT_MARGIN);
		}

		#region Control
		public void Execute()
		{
			Console.Execute(UserInput);
			UserInput = "";
		}
		#endregion
	}
}
