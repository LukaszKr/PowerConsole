using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.Editor.Test.Logic.Hint
{
	public class HintTest
	{
		private ConsoleInstance m_Console = new ConsoleInstance(new LocalizationManager(), null);
		protected HintState Hints { get { return m_Console.HintState; } }

		[Test]
		public void CommandNameHint()
		{
			//m_Console.Hints
		}
	}
}
