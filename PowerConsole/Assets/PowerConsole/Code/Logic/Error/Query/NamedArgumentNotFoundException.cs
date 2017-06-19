using System;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class NamedArgumentNotFoundException: Exception
	{
		public readonly string Name;
		public readonly string Value;

		public NamedArgumentNotFoundException(string name, string value)
		{
			Name = name;
			Value = value;
		}
	}
}
