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

		public Argument()
		{

		}
	}
}
