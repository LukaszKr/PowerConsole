namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class AHintIterator
	{
		public readonly Query Query;
		public readonly Argument Argument;
		public readonly AHint Hint; 

		public abstract string Current { get; }

		public AHintIterator(Query query, Argument argument, AHint hint)
		{
			Query = query;
			Argument = argument;
			Hint = hint;
		}

		public abstract bool MovePrev();
		public abstract bool MoveNext();
		public abstract void Restart();
	}
}
