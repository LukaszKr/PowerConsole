using System;

namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class AHint
	{
		public abstract Type HintedType { get; }

		public abstract AHintIterator GetIterator(Query query, Argument argument);
	}
}
