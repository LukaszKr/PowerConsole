namespace ProceduralLevel.PowerConsole.Logic
{
	public class Argument
	{
		private string _Name;

		public string Name 
		{ 
			get { return _Name; } 
			set { _Name = value.ToLowerInvariant(); }
		}

		public string Value;
		public object Parsed;
		public int Offset = -1;
		public CommandParameter Parameter;

		public bool IsMapped { get { return Parameter != null; } }

		public Argument()
		{

		}

		public override string ToString()
		{
			return string.Format("[Name: {0}, Value: {1}, Parsed: {2}, Parameter: {3}, Offset: {4}]",
				Name, Value, (Parsed != null? Parsed.ToString(): ""), (IsMapped? Parameter.ToString(): ""), Offset.ToString());
		}
	}
}
