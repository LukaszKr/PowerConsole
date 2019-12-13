namespace ProceduralLevel.PowerConsole.Logic
{
	public class LongHint: ANumberHint<long>
	{
		public LongHint() : base(long.MinValue, long.MaxValue)
		{
		}

		protected override long OnParse(string value)
		{
			long parsed;
			long.TryParse(value, out parsed);
			return parsed;
		}
	}
}
