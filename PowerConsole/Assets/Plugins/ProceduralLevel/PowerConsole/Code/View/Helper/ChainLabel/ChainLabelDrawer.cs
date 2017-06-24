using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ChainLabelDrawer
	{
		private List<ChainLabelEntry> m_Entries = new List<ChainLabelEntry>();

		public ChainLabelDrawer()
		{

		}

		public void AddEntry(ChainLabelEntry entry)
		{
			m_Entries.Add(entry);
		}

		public void Draw(Rect rect)
		{
			ChainLabelEntry prev = null;
			for(int x = 0; x < m_Entries.Count; x++)
			{
				ChainLabelEntry entry = m_Entries[x];
				entry.Render(rect, prev);
				prev = entry;
			}
		}

		public void MarkAsDirty()
		{
			for(int x = 0; x < m_Entries.Count; x++)
			{
				ChainLabelEntry entry = m_Entries[x];
				entry.IsDirty = true;
			}
		}
	}
}
