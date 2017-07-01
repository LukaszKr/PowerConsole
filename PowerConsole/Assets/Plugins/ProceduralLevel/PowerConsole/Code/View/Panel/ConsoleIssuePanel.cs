using ProceduralLevel.PowerConsole.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public delegate bool IssueHandler(Exception e, out string text);

	public class ConsoleIssuePanel: AConsolePanel
	{
		private Rect m_IssueRect;
		private GUIContent m_IssueContent = new GUIContent();

		private List<IssueHandler> m_Handlers;

		public ConsoleIssuePanel(ConsoleView consoleView) : base(consoleView)
		{
			m_Handlers = new List<IssueHandler>()
			{
				//Query
				CreateHandler<NamedArgumentNotFoundException>(NamedArgumentNotFoundHandler), 
				CreateHandler<NotEnoughtArgumentsException>(NotEnoughtArgumentsHandler),
				CreateHandler<QueryParserException>(QueryParserHandler),
				CreateHandler<TooManyArgumentsException>(TooManyArgumentsHandler),
				//Value
				CreateHandler<InvalidValueFormatException>(InvalidValueFormatHandler),
				CreateHandler<MissingValueParserException>(MissingValueParserHandler)
			};
		}

		public override float PreferedHeight(float availableHeight)
		{
			return (Console.InputState.Issues.Count > 0? Styles.InputHeight: 0);
		}

		protected override void OnRender(Vector2 size)
		{
			List<Exception> issues = Console.InputState.Issues;
			if(issues.Count > 0)
			{
				Exception issue = issues[0];
				bool handled = false;
				string text = string.Empty;

				for(int x = 0; x < m_Handlers.Count; x++)
				{
					IssueHandler handler = m_Handlers[x];
					handled = handler(issue, out text);
					if(handled)
					{
						break;
					}
				}
				if(!handled)
				{
					text = issue.ToString();
				}
				m_IssueContent.text = text;

				GUI.Label(m_IssueRect, m_IssueContent);
			}
		}

		protected override void OnSizeChanged(Vector2 size)
		{
			m_IssueRect = new Rect(0, 0, size.x, size.y);
		}

		#region Issue Handler Generator
		public delegate void IssueHandler<T>(T e, out string text);

		public static IssueHandler CreateHandler<T>(IssueHandler<T> handler) where T : Exception
		{
			return (Exception e, out string text) =>
			{
				T casted = e as T;
				if(casted == null)
				{
					text = null;
					return false;
				}
				else
				{
					handler(casted, out text);
					return true;
				}
			};
		}
		#endregion

		#region Query
		private void NamedArgumentNotFoundHandler(NamedArgumentNotFoundException e, out string text)
		{
			text = Localization.Get(ELocKey.LogicQueryNamedArgumentNotFound, e.Name);
		}

		private void NotEnoughtArgumentsHandler(NotEnoughtArgumentsException e, out string text)
		{
			text = Localization.Get(ELocKey.LogicQueryNotEnoughtArguments, e.Required, e.Parameters.Count);
		}

		private void QueryParserHandler(QueryParserException e, out string text)
		{
			text = Localization.Get(ELocKey.LogicParserError, e.ErrorCode, e.Token);
		}

		private void TooManyArgumentsHandler(TooManyArgumentsException e, out string text)
		{
			text = Localization.Get(ELocKey.LogicQueryTooManyArguments, e.Expected, e.Received);
		}
		#endregion

		#region Value
		private void InvalidValueFormatHandler(InvalidValueFormatException e, out string text)
		{
			text = Localization.Get(ELocKey.LogicParsingInvalidFormat, e.ExpectedType, e.RawValue);
		}

		private void MissingValueParserHandler(MissingValueParserException e, out string text)
		{
			text = Localization.Get(ELocKey.LogicParsingMissingParser, e.ExpectedType, e.RawValue);
		}
		#endregion
	}
}
