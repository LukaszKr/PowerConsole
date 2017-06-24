namespace ProceduralLevel.PowerConsole.Logic
{
	public class IntHint: ANumberHint<int>
	{
		public IntHint() : base(int.MinValue, int.MaxValue)
		{
		}

		protected override long Parse(string value)
		{
			int parsed;
			int.TryParse(value, out parsed);
			return parsed;
		}
	}
}
