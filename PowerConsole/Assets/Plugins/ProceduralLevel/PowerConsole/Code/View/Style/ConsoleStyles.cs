using ProceduralLevel.Common.Ext;
using ProceduralLevel.PowerConsole.Logic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleStyles
	{
		private bool m_Initialzed;

		//enum key allocates lots of memory

		private GUIStyle[] m_TextStyle = new GUIStyle[EnumExt.MaxValue<EMessageType>()+1];

		public GUIStyle this[EMessageType type]
		{
			get { return GetTextStyle(type); }
		}

		public GUIStyle Box { get; private set; }


		public GUIStyle DefaultText { get; private set; }
		public GUIStyle TitleText { get; private set; }
		public GUIStyle InputText { get; private set; }
		public GUIStyle HintText { get; private set; }
		public GUIStyle HintHitText { get; private set; }


		public float ScrollbarWidth { get; private set; }
		public float LineHeight { get; private set; }
		public float InputHeight { get; private set; }
		public float LineMargin { get; private set; }

		public void TryInitialize(bool force)
		{
			if(force || !m_Initialzed)
			{
				m_Initialzed = true;
				Initialize();
			}
		}

		private void Initialize()
		{
			ScrollbarWidth = GUI.skin.verticalScrollbar.fixedWidth;
			LineHeight = GUI.skin.label.lineHeight;

			GUIStyle style = GUI.skin.textField;
			LineMargin = style.margin.vertical+style.padding.vertical;
			InputHeight = style.lineHeight+LineMargin;

			Box = new GUIStyle("box");

			GenerateTextStyles();
		}

		private void GenerateTextStyles()
		{
			float offset = 0.2f;

			DefaultText = new GUIStyle("label");

			HintHitText = StyleFactory.TextStyle(0f, 0f, 0f, 1f, FontStyle.Bold);
			HintText = StyleFactory.TextStyle(-offset, -offset, -offset, 1f);

			TitleText = StyleFactory.TextStyle(0f, 0f, 0f, 1f, FontStyle.Bold, TextAnchor.MiddleCenter);
			InputText = StyleFactory.TextStyle(0f, 0f, 0f, 1f, FontStyle.Normal, TextAnchor.MiddleLeft);

			m_TextStyle[(int)EMessageType.Error] = StyleFactory.TextStyle(0f, -offset, -offset, 1f, FontStyle.Bold);
			m_TextStyle[(int)EMessageType.Execution] = StyleFactory.TextStyle(-offset, 0f, 0f, 1f, FontStyle.BoldAndItalic);
			m_TextStyle[(int)EMessageType.Info] = StyleFactory.TextStyle(-offset, -offset, 0f, 1f, FontStyle.Normal);
			m_TextStyle[(int)EMessageType.Success] = StyleFactory.TextStyle(-offset, 0f, -offset, 1f, FontStyle.Normal);
			m_TextStyle[(int)EMessageType.Warning] = StyleFactory.TextStyle(0f, 0, -offset, 1f, FontStyle.Italic);
			m_TextStyle[(int)EMessageType.Normal] = StyleFactory.TextStyle(0f, 0f, 0f, 1f, FontStyle.Normal);
		}

		private GUIStyle GetTextStyle(EMessageType type)
		{
			return m_TextStyle[(int)type];
		}
	}
}
