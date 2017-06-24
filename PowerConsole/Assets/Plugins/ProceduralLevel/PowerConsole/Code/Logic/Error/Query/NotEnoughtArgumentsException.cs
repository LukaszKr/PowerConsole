using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class NotEnoughtArgumentsException: ConsoleException
	{
		public readonly List<CommandParameter> Parameters;

		public NotEnoughtArgumentsException(List<CommandParameter> parameters)
		{
			Parameters = new List<CommandParameter>(parameters);
		}
	}
}
