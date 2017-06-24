namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class AHintIterator
	{
		public abstract string Current { get; }

		public abstract bool MovePrev();
		public abstract bool MoveNext();
		public abstract void Restart();
	}
}
