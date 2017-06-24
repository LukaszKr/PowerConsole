namespace ProceduralLevel.PowerConsole.Logic
{
	public class UShortHint: ANumberHint<ushort>
	{
		public UShortHint() : base(ushort.MinValue, ushort.MaxValue)
		{
		}

		protected override long Parse(string value)
		{
			ushort parsed;
			ushort.TryParse(value, out parsed);
			return parsed;
		}
	}
}
