using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class HintEnumerator
	{
		private List<string> m_Hints;
		private int m_Current;

		public string Current
		{
			get 
			{ 
				if(m_Hints.Count > m_Current)
				{
					return m_Hints[m_Current];
				}
				return string.Empty;
			}
		}

		public HintEnumerator(List<string> hints)
		{
			m_Hints = hints;
		}

		public bool MoveNext()
		{
			m_Current ++;
			return(m_Current < m_Hints.Count);
		}

		public void Restart()
		{
			m_Current = 0;
		}
	}
}
