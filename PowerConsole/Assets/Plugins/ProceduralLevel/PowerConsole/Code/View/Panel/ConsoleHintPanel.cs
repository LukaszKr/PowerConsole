using ProceduralLevel.Common.Event;
using ProceduralLevel.PowerConsole.Logic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleHintPanel: AConsolePanel
	{
		private float H_MARGIN = 4f;
		private float V_MARGIN = 2f;
		private const string COMMAND_HINT = "{0} - {1}";
		private const string PARAMETER_HINT = "{0}[{1}] - ";

		private Rect m_CommandRect;
		private Rect m_ParameterRect;

		private GUIContent m_CommandLabel = new GUIContent();

		private ChainLabelDrawer m_HintDrawer = new ChainLabelDrawer();

		private GUIContent m_ParameterLabel = new GUIContent();
		private GUIContent m_HintPrefixLabel = new GUIContent();
		private GUIContent m_HintHitLabel = new GUIContent();
		private GUIContent m_HintSufixLabel = new GUIContent();

		public Event<HintHit> OnHitChanged = new Event<HintHit>();

		public ConsoleHintPanel(ConsoleView consoleView) : base(consoleView)
		{
			Hint.OnHintChanged.AddListener(HintChangedHandler);
			HintChangedHandler(Hint.Current);
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
			if(Hint.Iterator != null)
			{
				height += Styles.LineHeight*2;
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
			if(Hint.Iterator != null)
			{
				GUI.Label(m_ParameterRect, m_ParameterLabel);
				m_HintDrawer.Draw(m_ParameterRect);
			}
		}

		protected override void OnSizeChanged(Vector2 size)
		{
			m_CommandRect = new Rect(H_MARGIN, V_MARGIN, size.x-H_MARGIN*2, size.y-V_MARGIN*2);
			m_ParameterRect = new Rect(H_MARGIN, V_MARGIN+Styles.LineHeight, size.x-H_MARGIN*2, size.y-V_MARGIN*2);
			m_HintDrawer.MarkAsDirty();
		}

		private void HintChangedHandler(HintHit hit)
		{
			AHintIterator iterator = Hint.Iterator;
			if(iterator.Argument.IsCommandName)
			{
				AConsoleCommand command = Console.FindCommand(iterator.Current);
				if(command != null)
				{
					m_CommandLabel.text = string.Format(COMMAND_HINT, command.ToString(), command.Description);
				}
				else
				{
					m_CommandLabel.text = iterator.Argument.Value;
				}
			}
			else
			{
				m_CommandLabel.text = iterator.Query.Name.Value;
			}
			if(hit != null && iterator.Query != null && iterator.Hint != null)
			{
				m_ParameterLabel.text = string.Format(PARAMETER_HINT, iterator.Argument.Name, iterator.Hint.HintedType.Name);
				m_HintPrefixLabel.text = hit.HitPrefix;
				m_HintHitLabel.text = hit.Value;
				m_HintSufixLabel.text = hit.HitSufix;
				m_HintDrawer.MarkAsDirty();
			}
		}
	}
}
