using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class AHint
	{
		public abstract Type HintedType { get; }

		protected abstract List<string> GetHints(string value);

		public HintEnumerator GetEnumerator(string value)
		{
			return new HintEnumerator(GetHints(value));
		}
	}
}
