using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleIssuePanel: AConsolePanel
	{
		private Rect m_IssueRect;

		public ConsoleIssuePanel(ConsoleView consoleView) : base(consoleView)
		{
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
				GUI.Label(m_IssueRect, issue.ToString());
			}
		}

		protected override void OnSizeChanged(Vector2 size)
		{
			m_IssueRect = new Rect(0, 0, size.x, size.y);
		}
	}
}
