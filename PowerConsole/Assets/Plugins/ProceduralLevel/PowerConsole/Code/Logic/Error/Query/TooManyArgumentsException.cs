namespace ProceduralLevel.PowerConsole.Logic
{
	public class TooManyArgumentsException: ConsoleException
	{
		public readonly int Received;
		public readonly int Expected;

		public TooManyArgumentsException(int received, int expected)
		{
			Received = received;
			Expected = expected;
		}

		public override string ToString()
		{
			return string.Format("[Received: {0}, Expected: {1}]", Received, Expected);
		}
	}
}
