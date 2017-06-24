namespace ProceduralLevel.PowerConsole.Logic
{
	public class NoHint: ADynamicHint<string>
	{
		public override string PrevHint(string value)
		{
			return value;
		}

		public override string NextHint(string value)
		{
			return value;
		}
	}
}
