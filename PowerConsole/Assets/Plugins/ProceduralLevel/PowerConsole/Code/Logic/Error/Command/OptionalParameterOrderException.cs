namespace ProceduralLevel.PowerConsole.Logic
{
	public class OptionalParameterOrderException: ConsoleException
	{
		public readonly CommandMethod Method;
		public readonly CommandParameter Optional;
		public readonly CommandParameter NonOptional;

		public OptionalParameterOrderException(CommandMethod method, CommandParameter optionalParameter, CommandParameter nonOptional)
		{
			Method = method;
			Optional = optionalParameter;
			NonOptional = nonOptional;
		}

		public override string ToString()
		{
			return string.Format("(Optional: {0}, NonOptional: {1})", Optional.ToString(), NonOptional.ToString());
		}
	}
}
