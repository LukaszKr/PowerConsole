using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class NotEnoughtArgumentsException: ConsoleException
	{
		public readonly Query Query;
		public readonly int Required;
		public readonly List<CommandParameter> Parameters;

		public NotEnoughtArgumentsException(Query query, int required, List<CommandParameter> parameters)
		{
			Query = query;
			Required = required;
			Parameters = new List<CommandParameter>(parameters);
		}

		public override string ToString()
		{
			return string.Format("[Required: {0}, Query: {1}, Missing: {2}", Required.ToString(), Query.ToString(), Parameters.Count.ToString());
		}
	}
}
