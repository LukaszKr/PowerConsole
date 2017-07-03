using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class ACollectionHint<HintType>: AGenericHint<HintType>
	{
		private string[] m_Cache;
		private bool m_IsValid;

		private List<string> GetHints(string value)
		{
			if(!m_IsValid)
			{
				m_IsValid = true;
				m_Cache = GetAllOptions();
			}
			List<string> result = new List<string>();
			if(string.IsNullOrEmpty(value))
			{
				for(int x = 0; x < m_Cache.Length; x++)
				{
					result.Add(m_Cache[x]);
				}
			}
			else
			{
				for(int x = 0; x < m_Cache.Length; x++)
				{
					string cached = m_Cache[x];
					if(cached.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
					{
						result.Add(cached);
					}
				}
			}

			result.Sort(SortStrategy);

			return result;
		}

		protected abstract string[] GetAllOptions();

		public void InvalidateCache()
		{
			m_IsValid = false;
		}

		private int SortStrategy(string a, string b)
		{
			int compare = a.CompareTo(b);
			if(compare == 0)
			{
				compare = a.Length.CompareTo(b.Length);
			}
			return compare;
		}

		public override AHintIterator GetIterator(Query query, Argument argument)
		{
			return new CollectionHintIterator(query, argument, this, GetHints(argument.Value));
		}
	}
}
