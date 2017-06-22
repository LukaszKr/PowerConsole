using ProceduralLevel.PowerConsole.Logic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleStyles
	{
		private bool m_Initialzed;

		//enum key allocates lots of memory

		private GUIStyle[] m_TextStyle = new GUIStyle[5];
		private GUIStyle m_TextError;
		private GUIStyle m_TextExecution;
		private GUIStyle m_TextInfo;
		private GUIStyle m_TextSuccess;
		private GUIStyle m_TextWarning;

		public GUIStyle this[EMessageType type]
		{
			get { return GetTextStyle(type); }
		}

		public GUIStyle Box { get; private set; }


		public float ScrollbarWidth { get; private set; }

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

			Box = new GUIStyle("box");

			float offset = 0.2f;
			m_TextStyle[(int)EMessageType.Error] = StyleFactory.TextStyle(0f, -offset, -offset, FontStyle.Bold);
			m_TextStyle[(int)EMessageType.Execution] = StyleFactory.TextStyle(-offset, 0f, 0f, FontStyle.BoldAndItalic);
			m_TextStyle[(int)EMessageType.Info] = StyleFactory.TextStyle(-offset, -offset, 0f, FontStyle.Normal);
			m_TextStyle[(int)EMessageType.Success] = StyleFactory.TextStyle(-offset, 0f, -offset, FontStyle.Normal);
			m_TextStyle[(int)EMessageType.Warning] = StyleFactory.TextStyle(0f, 0, -offset, FontStyle.Italic);
		}

		private GUIStyle GetTextStyle(EMessageType type)
		{
			return m_TextStyle[(int)type];
		}
	}
}
