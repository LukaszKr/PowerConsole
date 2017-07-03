using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class CollectionHintIterator: AHintIterator
	{
		private List<string> m_Hints;
		private int m_Current;

		public override string Current
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

		public CollectionHintIterator(Query query, Argument argument, AHint hint, List<string> hintsList)
			: base(query, argument, hint)
		{
			Restart();
			m_Hints = hintsList;
		}

		public override bool MovePrev()
		{
			m_Current--;
			return (m_Current >= 0);
		}

		public override bool MoveNext()
		{
			m_Current++;
			return (m_Current < m_Hints.Count);
		}

		public override void Restart()
		{
			m_Current = 0;
		}
	}
}
