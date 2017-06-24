using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class CommandNameHint: ACollectionHint
	{
		public override Type HintedType { get { return typeof(string); } }

		private List<AConsoleCommand> m_Commands;

		public CommandNameHint(List<AConsoleCommand> commands)
		{
			m_Commands = commands;
		}

		protected override string[] GetAllOptions()
		{
			string[] options = new string[m_Commands.Count];

			for(int x = 0; x < m_Commands.Count; x++)
			{
				AConsoleCommand command = m_Commands[x];
				options[x] = command.Name;
			}

			return options;
		}
	}
}
