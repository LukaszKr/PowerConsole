using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.Editor.Test.Logic.Hint
{
    [Category("PowerConsole")]
    public class HintTest
	{
		private ConsoleInstance m_Console = new ConsoleInstance(new LocalizationManager(), null);
		protected HintModule Hints { get { return m_Console.HintModule; } }

		[Test]
		public void CommandNameHint()
		{
			//m_Console.Hints
		}
	}
}
