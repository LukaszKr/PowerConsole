namespace ProceduralLevel.PowerConsole.Logic
{
	public class NamedArgumentNotFoundException: ConsoleException
	{
		public readonly string Name;
		public readonly string Value;

		public NamedArgumentNotFoundException(string name, string value)
		{
			Name = name;
			Value = value;
		}

		public override string ToString()
		{
			return string.Format("[Name: {0}, Value: {1}]", Name, Value);
		}
	}
}
