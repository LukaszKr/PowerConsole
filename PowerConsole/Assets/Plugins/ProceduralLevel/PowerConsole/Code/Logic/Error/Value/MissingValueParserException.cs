using System;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class MissingValueParserException: ConsoleException
	{
		public readonly Type ExpectedType;
		public readonly string RawValue;

		public MissingValueParserException(Type expectedType, string rawValue)
		{
			ExpectedType  = expectedType;
			RawValue = rawValue;
		}

		public override string ToString()
		{
			return string.Format("[ExpectedType: {0}, RawValue: {1}]", ExpectedType.Name, RawValue);
		}
	}
}
