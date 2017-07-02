namespace ProceduralLevel.PowerConsole.Logic
{
	public class FloatHint: ADynamicHint<float>
	{
		protected float m_Min;
		protected float m_Max;

		public FloatHint()
		{
			m_Min = float.MinValue;
			m_Max = float.MaxValue;
		}

		public override string NextHint(string value)
		{
			float parsed = Parse(value);
			if(parsed < m_Max)
			{
				return (parsed+1).ToString();
			}
			return string.Empty;
		}

		public override string PrevHint(string value)
		{
			float parsed = Parse(value);
			if(parsed > m_Min)
			{
				return (parsed-1).ToString();
			}
			return string.Empty;
		}

		protected float Parse(string value)
		{
			float parsed;
			float.TryParse(value, out parsed);
			return parsed;
		}
	}
}
