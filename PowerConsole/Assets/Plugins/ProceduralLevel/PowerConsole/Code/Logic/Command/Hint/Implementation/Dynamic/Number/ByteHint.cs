namespace ProceduralLevel.PowerConsole.Logic
{
	public class ByteHint: ANumberHint<byte>
	{
		public ByteHint() : base(byte.MinValue, byte.MaxValue)
		{
		}

		protected override long OnParse(string value)
		{
			byte parsed;
			byte.TryParse(value, out parsed);
			return parsed;
		}
	}
}
