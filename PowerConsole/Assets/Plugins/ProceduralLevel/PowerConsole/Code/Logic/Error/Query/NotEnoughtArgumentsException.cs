using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class NotEnoughtArgumentsException: Exception
	{
		public readonly List<CommandParameter> Parameters;

		public NotEnoughtArgumentsException(List<CommandParameter> parameters)
		{
			Parameters = new List<CommandParameter>(parameters);
		}
	}
}
