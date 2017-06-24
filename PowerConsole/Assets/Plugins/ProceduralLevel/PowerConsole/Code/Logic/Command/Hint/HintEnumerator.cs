using System.Collections;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class HintEnumerator: IEnumerator
	{
		private string[] m_Hints;
		private int m_Current;

		public object Current
		{
			get { return m_Hints[m_Current]; }
		}

		public HintEnumerator(string[] hints)
		{
			m_Hints = hints;
		}

		public bool MoveNext()
		{
			m_Current ++;
			return(m_Current < m_Hints.Length);
		}

		public void Reset()
		{
			m_Current = 0;
		}
	}
}
