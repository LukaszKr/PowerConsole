namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class ANumberHint<NumberType>: ADynamicHint<NumberType>
	{
		protected long m_Min;
		protected long m_Max;

		public ANumberHint(long min, long max)
		{
			m_Min = min;
			m_Max = max;
		}

		public override string NextHint(string value)
		{
			long parsed = Parse(value);
			if(parsed < m_Max)
			{
				return (parsed+1).ToString();
			}
			return string.Empty;
		}

		public override string PrevHint(string value)
		{
			long parsed = Parse(value);
			if(parsed > m_Min)
			{
				return (parsed-1).ToString();
			}
			return string.Empty;
		}

		protected abstract long Parse(string value);
	}
}
