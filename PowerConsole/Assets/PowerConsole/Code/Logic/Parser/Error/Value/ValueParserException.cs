using System;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class ValueParserException: Exception
	{
		public readonly Type ExpectedType;
		public readonly string RawValue;

		public ValueParserException(Type expectedType, string rawValue)
		{
			ExpectedType  = expectedType;
			RawValue = rawValue;
		}

		public override string ToString()
		{
			return string.Format("[ExpectedType: {0}, RawValue: {1}", ExpectedType.Name, RawValue);
		}
	}
}
