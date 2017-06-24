using ProceduralLevel.PowerConsole.Logic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleHintPanel: AConsolePanel
	{
		private float H_MARGIN = 4f;
		private float V_MARGIN = 2f;
		private const string COMMAND_HINT = "{0} - {1}";
		private const string PARAMETER_HINT = "{0}[{1}] - {2}";

		private Argument m_Argument;

		private AConsoleCommand m_Command;
		private AHint m_Hint;
		private HintEnumerator m_Enumerator;

		private Rect m_CommandRect;
		private Rect m_ParameterRect;
		private GUIContent m_CommandLabel = new GUIContent();
		private GUIContent m_ParameterLabel = new GUIContent();

		public ConsoleHintPanel(ConsoleView consoleView) : base(consoleView)
		{
		}

		public override float PreferedHeight(float availableHeight)
		{
			return Styles.LineHeight*2+Styles.LineMargin;
		}

		protected override void OnRender(Vector2 size)
		{
			GUI.Label(m_CommandRect, m_CommandLabel);
			GUI.Label(m_ParameterRect, m_ParameterLabel);
		}

		protected override void OnSizeChanged(Vector2 size)
		{
			m_CommandRect = new Rect(H_MARGIN, V_MARGIN, size.x-H_MARGIN*2, size.y-V_MARGIN*2);
			m_ParameterRect = new Rect(H_MARGIN, V_MARGIN+Styles.LineHeight, size.x-H_MARGIN*2, size.y-V_MARGIN*2);
		}

		public void UpdateHint(AConsoleCommand command, Argument argument)
		{
			if(m_Argument != argument)
			{
				m_Argument = argument;
				if(m_Argument != null)
				{
					if(m_Argument.Parameter != null)
					{
						m_Hint = command.GetHintFor(Console.Hints, m_Argument.Parameter.Index);
						m_Enumerator = m_Hint.GetEnumerator(argument.Value);
					}
					else if(argument.IsCommandName)
					{
						m_Hint = Console.NameHint;
						m_Enumerator = m_Hint.GetEnumerator(argument.Value);
						m_Command = Console.FindCommand(m_Enumerator.Current);
					}
				}
				else
				{
					m_Hint = null;
					m_Enumerator = null;
				}
				RefreshHint();
			}
		}

		public void NextHint()
		{
			if(!m_Enumerator.MoveNext())
			{
				m_Enumerator.Restart();
			}
		}

		private void RefreshHint()
		{
			if(m_Argument != null)
			{ 
				if(m_Command != null)
				{
					m_CommandLabel.text = string.Format(COMMAND_HINT, m_Command.ToString(), m_Command.Description);
				}
				else
				{
					m_CommandLabel.text = m_Argument.Value;
				}
				if(m_Hint != null && m_Argument.Parameter != null)
				{
					m_ParameterLabel.text = string.Format(PARAMETER_HINT, m_Argument.Name, m_Hint.HintedType.Name, m_Enumerator.Current);
				}
				else
				{
					m_ParameterLabel.text = "";
				}
			}
		}
	}
}
