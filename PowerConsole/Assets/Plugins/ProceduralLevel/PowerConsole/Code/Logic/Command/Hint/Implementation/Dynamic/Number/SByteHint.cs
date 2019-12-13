﻿namespace ProceduralLevel.PowerConsole.Logic
{
	public class SByteHint: ANumberHint<sbyte>
	{
		public SByteHint(): base(sbyte.MinValue, sbyte.MaxValue)
		{
		}

		protected override long OnParse(string value)
		{
			sbyte parsed;
			sbyte.TryParse(value, out parsed);
			return parsed;
		}
	}
}
