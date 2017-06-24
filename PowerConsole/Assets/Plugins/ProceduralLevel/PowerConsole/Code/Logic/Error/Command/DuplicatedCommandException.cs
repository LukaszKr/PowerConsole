namespace ProceduralLevel.PowerConsole.Logic
{
	public class DuplicatedCommandException: ConsoleException
	{
		public readonly AConsoleCommand Existing;
		public readonly AConsoleCommand Duplicating;

		public DuplicatedCommandException(AConsoleCommand existing, AConsoleCommand duplicating)
		{
			Existing = existing;
			Duplicating = duplicating;
		}

		public override string ToString()
		{
			return string.Format("[Existing: {0}, Duplicating: {1}]", Existing.ToString(), Duplicating.ToString());
		}
	}
}
