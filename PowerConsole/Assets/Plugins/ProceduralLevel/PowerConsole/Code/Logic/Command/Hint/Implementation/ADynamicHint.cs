namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class ADynamicHint<HintType>: AGenericHint<HintType>
	{
		public abstract string PrevHint(string value);
		public abstract string NextHint(string value);

		public override AHintIterator GetIterator(string value)
		{
			return new DynamicHintIterator<HintType>(this, value);
		}
	}
}
