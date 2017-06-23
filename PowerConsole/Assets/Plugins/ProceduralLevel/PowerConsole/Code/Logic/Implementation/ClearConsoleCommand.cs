using System;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class ClearConsoleCommand: AConsoleCommand
	{
		public ClearConsoleCommand(string name, string description) : base(name, description)
		{
		}

		public override bool IsValid()
		{
			throw new NotImplementedException();
		}
	}
}
