namespace ProceduralLevel.PowerConsole.Logic
{
	public class ShortHint: ANumberHint<short>
	{
		public ShortHint() : base(short.MinValue, short.MaxValue)
		{
		}

		protected override long OnParse(string value)
		{
			short parsed;
			short.TryParse(value, out parsed);
			return parsed;
		}
	}
}
