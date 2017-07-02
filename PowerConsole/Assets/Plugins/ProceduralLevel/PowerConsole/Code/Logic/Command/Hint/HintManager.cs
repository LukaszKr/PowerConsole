using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class HintManager
	{
		private List<AHint> m_Hints = new List<AHint>();

		private NoHint m_NoHint = new NoHint();

		public HintManager()
		{
			RegisterHint(new ByteHint());
			RegisterHint(new ShortHint());
			RegisterHint(new IntHint());
			RegisterHint(new LongHint());

			RegisterHint(new SByteHint());
			RegisterHint(new UShortHint());
			RegisterHint(new UIntHint());
			//RegisterHint(new ULongHint());

			RegisterHint(new FloatHint());
		}

		public AHint GetHint(Type type)
		{
			int index = IndexOfTypeHint(type);
			if(index >= 0)
			{
				return m_Hints[index];
			}
			if(type.IsEnum)
			{
				EnumHint hint = new EnumHint(type);
				RegisterHint(hint);
				return hint;
			}
			return m_NoHint;
		}

		private int IndexOfTypeHint(Type type)
		{
			for(int x = 0; x < m_Hints.Count; x++)
			{
				AHint hint = m_Hints[x];
				if(hint.HintedType == type)
				{
					return x;
				}
			}
			return -1;
		}

		//if hint for given type already exists, overwrite it
		public void RegisterHint(AHint hint)
		{
			int index = IndexOfTypeHint(hint.HintedType);
			if(index >= 0)
			{
				m_Hints.RemoveAt(index);
			}
			m_Hints.Add(hint);
		}
	}
}
