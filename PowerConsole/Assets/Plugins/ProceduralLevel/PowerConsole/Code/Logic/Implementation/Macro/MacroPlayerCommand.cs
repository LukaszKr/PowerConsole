namespace ProceduralLevel.PowerConsole.Logic
{
	public class MacroPlayerCommand: AConsoleCommand
	{
		private Macro m_Macro;

		public MacroPlayerCommand(ConsoleInstance console, Macro macro) : base(console, macro.Name, "")
		{
			m_Macro = macro;
		}

		public Message Command()
		{
			for(int x = 0; x < m_Macro.Queries.Count; x++)
			{
				Console.Execute(m_Macro.Queries[x]);
			}
			return null;
		}
	}
}
