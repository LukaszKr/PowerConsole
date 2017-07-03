namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class ADynamicHint<HintType>: AGenericHint<HintType>
	{
		public abstract string PrevHint(string value);
		public abstract string NextHint(string value);

		public override AHintIterator GetIterator(Query query, Argument argument)
		{
			return new DynamicHintIterator<HintType>(query, argument, this);
		}
	}
}
