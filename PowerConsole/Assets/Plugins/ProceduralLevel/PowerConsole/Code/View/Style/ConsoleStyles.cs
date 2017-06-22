using ProceduralLevel.PowerConsole.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleStyles
	{
		private bool m_Initialzed;

		//enum key allocates lots of memory
		private Dictionary<int, GUIStyle> m_TextStyle = new Dictionary<int, GUIStyle>();

		public GUIStyle this[EMessageType type]
		{
			get { return m_TextStyle[(int)type]; }
		}

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

			float offset = 0.2f;
			m_TextStyle[(int)EMessageType.Error] = StyleFactory.TextStyle(0f, -offset, -offset, FontStyle.Bold);
			m_TextStyle[(int)EMessageType.Execution] = StyleFactory.TextStyle(-offset, 0f, 0f, FontStyle.BoldAndItalic);
			m_TextStyle[(int)EMessageType.Info] = StyleFactory.TextStyle(-offset, -offset, 0f, FontStyle.Normal);
			m_TextStyle[(int)EMessageType.Success] = StyleFactory.TextStyle(-offset, 0f, -offset, FontStyle.Normal);
			m_TextStyle[(int)EMessageType.Warning] = StyleFactory.TextStyle(0f, 0, -offset, FontStyle.Italic);
		}
	}
}
