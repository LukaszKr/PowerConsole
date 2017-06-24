using ProceduralLevel.PowerConsole.Logic;
using System;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleHintPanel: AConsolePanel
	{
		private float H_MARGIN = 4f;
		private float V_MARGIN = 2f;
		private const string COMMAND_HINT = "{0} - {1}";
		private const string PARAMETER_HINT = "{0}[{1}] - ";

		private Argument m_Argument;

		private AConsoleCommand m_Command;
		private AHint m_Hint;
		private AHintIterator m_Enumerator;

		private Rect m_CommandRect;
		private Rect m_ParameterRect;

		private GUIContent m_CommandLabel = new GUIContent();

		private ChainLabelDrawer m_HintDrawer = new ChainLabelDrawer();

		private GUIContent m_ParameterLabel = new GUIContent();
		private GUIContent m_HintPrefixLabel = new GUIContent();
		private GUIContent m_HintHitLabel = new GUIContent();
		private GUIContent m_HintSufixLabel = new GUIContent();


		public ConsoleHintPanel(ConsoleView consoleView) : base(consoleView)
		{
		}

		protected override void Initialize()
		{
			base.Initialize();

			m_HintDrawer.AddEntry(new ChainLabelEntry(m_ParameterLabel, Styles.DefaultText));
			m_HintDrawer.AddEntry(new ChainLabelEntry(m_HintPrefixLabel, Styles.HintText));
			m_HintDrawer.AddEntry(new ChainLabelEntry(m_HintHitLabel, Styles.HintHitText));
			m_HintDrawer.AddEntry(new ChainLabelEntry(m_HintSufixLabel, Styles.HintText));
		}

		public override float PreferedHeight(float availableHeight)
		{
			float height = 0;
			if(m_Command != null)
			{
				height += Styles.LineHeight;
			}
			if(DisplayParameter())
			{
				height += Styles.LineHeight;
			}
			if(height > 0)
			{
				height += Styles.LineMargin;
			}
			return height;
		}

		protected override void OnRender(Vector2 size)
		{
			GUI.Label(m_CommandRect, m_CommandLabel);
			if(DisplayParameter())
			{
				m_HintDrawer.Draw(m_ParameterRect);
				GUI.Label(m_ParameterRect, m_ParameterLabel);
			}
		}

		protected override void OnSizeChanged(Vector2 size)
		{
			m_CommandRect = new Rect(H_MARGIN, V_MARGIN, size.x-H_MARGIN*2, size.y-V_MARGIN*2);
			m_ParameterRect = new Rect(H_MARGIN, V_MARGIN+Styles.LineHeight, size.x-H_MARGIN*2, size.y-V_MARGIN*2);
			m_HintDrawer.MarkAsDirty();
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
						m_Enumerator = m_Hint.GetIterator(argument.Value);
					}
					else if(argument.IsCommandName)
					{
						m_Hint = Console.NameHint;
						m_Enumerator = m_Hint.GetIterator(argument.Value);
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
				if(DisplayParameter())
				{
					m_ParameterLabel.text = string.Format(PARAMETER_HINT, m_Argument.Name, m_Hint.HintedType.Name);

					string hintText = m_Enumerator.Current;
					string argText = m_Argument.Value;
					int hitIndex =  hintText.IndexOf(argText, StringComparison.OrdinalIgnoreCase);

					m_HintPrefixLabel.text = hintText.Substring(0, hitIndex);
					m_HintHitLabel.text = argText;
					m_HintSufixLabel.text = hintText.Substring(hitIndex+argText.Length);
					m_HintDrawer.MarkAsDirty();
				}
				else
				{
					m_ParameterLabel.text = "";
					m_HintHitLabel.text = "";
				}
			}
		}

		private bool DisplayParameter()
		{
			return (m_Hint != null);
		}
	}
}
