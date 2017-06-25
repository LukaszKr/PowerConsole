using ProceduralLevel.PowerConsole.Logic;
using ProceduralLevel.UnityCommon.Ext;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleInputPanel: AConsolePanel
	{
		private const int PADDING = 6;
		private const int SUBMIT_MARGIN = 2;

		private GUIContent m_ButtonText;

		private Rect m_InputRect;
		private Rect m_SuggestedRect;
		private Rect m_SubmitRect;

		private ChainLabelDrawer m_Suggested = new ChainLabelDrawer();
		private GUIContent m_Prefix = new GUIContent();
		private GUIContent m_HintPrefix = new GUIContent();
		private GUIContent m_HintHit = new GUIContent();
		private GUIContent m_HintSufix = new GUIContent();
		private GUIContent m_Sufix = new GUIContent();

		public ConsoleInputPanel(ConsoleView consoleView) : base(consoleView)
		{
			m_ButtonText = new GUIContent(Localization.GetLocalizedKey(ELocalizationKey.Input_Submit));

			Console.HintState.OnHintChanged.AddListener(HintChangedHandler);
		}

		protected override void Initialize()
		{
			base.Initialize();

			m_Suggested.AddEntry(new ChainLabelEntry(m_Prefix, Styles.InvisibleText));
			m_Suggested.AddEntry(new ChainLabelEntry(m_HintPrefix, Styles.HintText));
			m_Suggested.AddEntry(new ChainLabelEntry(m_HintHit, Styles.InvisibleText));
			m_Suggested.AddEntry(new ChainLabelEntry(m_HintSufix, Styles.HintText));
			m_Suggested.AddEntry(new ChainLabelEntry(m_Sufix, Styles.InvisibleText));
		}

		public override float PreferedHeight(float availableHeight)
		{
			return Styles.InputHeight;
		}

		protected override void OnRender(Vector2 size)
		{
			string newInput = GUI.TextField(m_InputRect, Console.InputState.UserInput, Styles.InputText);
			if(Console.HintState.IteratingHints)
			{
				m_Suggested.Draw(m_SuggestedRect);
			}
			if(GUI.Button(m_SubmitRect, m_ButtonText))
			{
				Console.InputState.Execute();
			}
			int newCursor = TextEditorHelper.GetCursor();
			Console.InputState.SetInput(newInput, newCursor);
		}

		protected override void OnSizeChanged(Vector2 size)
		{
			float inputWidth = size.x*0.8f;
			m_InputRect = new Rect(PADDING, 0, inputWidth-PADDING*2, size.y);
			m_SubmitRect = new Rect(inputWidth, 0, size.x-inputWidth, size.y).AddMargin(SUBMIT_MARGIN, SUBMIT_MARGIN);
			m_SuggestedRect = new Rect(m_InputRect.x, m_InputRect.y+2, m_InputRect.width, m_InputRect.height);
		}

		private void HintChangedHandler(HintHit hit)
		{
			if(hit != null)
			{
				m_Prefix.text = hit.Prefix;
				m_HintPrefix.text = hit.HitPrefix;
				m_HintHit.text = hit.Value;
				m_HintSufix.text = hit.HitSufix;
				m_Sufix.text = hit.Sufix;

				m_Suggested.MarkAsDirty();
			}
		}
	}
}
