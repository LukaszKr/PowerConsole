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
		public GUIStyle TitleText { get; private set; }
		public GUIStyle InputText { get; private set; }


		public float ScrollbarWidth { get; private set; }
		public float LineHeight { get; private set; }
		public float InputHeight { get; private set; }

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
			InputHeight = style.lineHeight+style.padding.vertical+style.margin.vertical;

			Box = new GUIStyle("box");

			TitleText = StyleFactory.TextStyle(0f, 0f, 0f, FontStyle.Bold, TextAnchor.MiddleCenter);
			InputText = StyleFactory.TextStyle(0f, 0f, 0f, FontStyle.Normal, TextAnchor.MiddleLeft);

			float offset = 0.2f;
			m_TextStyle[(int)EMessageType.Error] = StyleFactory.TextStyle(0f, -offset, -offset, FontStyle.Bold);
			m_TextStyle[(int)EMessageType.Execution] = StyleFactory.TextStyle(-offset, 0f, 0f, FontStyle.BoldAndItalic);
			m_TextStyle[(int)EMessageType.Info] = StyleFactory.TextStyle(-offset, -offset, 0f, FontStyle.Normal);
			m_TextStyle[(int)EMessageType.Success] = StyleFactory.TextStyle(-offset, 0f, -offset, FontStyle.Normal);
			m_TextStyle[(int)EMessageType.Warning] = StyleFactory.TextStyle(0f, 0, -offset, FontStyle.Italic);
			m_TextStyle[(int)EMessageType.Announcement] = StyleFactory.TextStyle(0f, 0f, 0f, FontStyle.Bold, TextAnchor.UpperCenter);
		}

		private GUIStyle GetTextStyle(EMessageType type)
		{
			return m_TextStyle[(int)type];
		}
	}
}
