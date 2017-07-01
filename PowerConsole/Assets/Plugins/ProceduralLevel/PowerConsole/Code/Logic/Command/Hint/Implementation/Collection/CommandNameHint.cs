using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class CommandNameHint: ACollectionHint<string>
	{
		private List<AConsoleCommand> m_Commands;
		private bool m_IsOption;

		public CommandNameHint(List<AConsoleCommand> commands, bool isOption)
		{
			m_Commands = commands;
			m_IsOption = isOption;
		}

		protected override string[] GetAllOptions()
		{
			List<string> options = new List<string>(m_Commands.Count);

			for(int x = 0; x < m_Commands.Count; x++)
			{
				AConsoleCommand command = m_Commands[x];
				if(command.IsValid && command.IsOption == m_IsOption)
				{
					options.Add(command.Name);
				}
			}

			return options.ToArray();
		}
	}
}
