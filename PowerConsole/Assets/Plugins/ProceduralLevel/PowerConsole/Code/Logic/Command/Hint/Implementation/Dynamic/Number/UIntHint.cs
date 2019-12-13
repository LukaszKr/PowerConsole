namespace ProceduralLevel.PowerConsole.Logic
{
	public class UIntHint: ANumberHint<uint>
	{
		public UIntHint() : base(uint.MinValue, uint.MaxValue)
		{
		}

		protected override long OnParse(string value)
		{
			uint parsed;
			uint.TryParse(value, out parsed);
			return parsed;
		}
	}
}
