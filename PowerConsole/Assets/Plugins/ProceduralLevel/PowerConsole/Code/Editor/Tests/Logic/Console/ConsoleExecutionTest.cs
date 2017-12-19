using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.Editor.Test.Logic.Console
{
    [Category("PowerConsole")]
    public class ConsoleExecutionTest
	{
		private const int TEST_INT = 123;
		private const bool TEST_BOOL = true;
		private const string TEST_STRING = "abcd";

		private ConsoleInstance m_Console = new ConsoleInstance(new LocalizationManager(), null);
		private TestCommand m_Command;
		private Message m_Message;

		public ConsoleExecutionTest()
		{
			m_Command = new TestCommand(m_Console);
			m_Console.AddCommand(m_Command);
			m_Message = m_Command.Command(TEST_INT, TEST_BOOL, TEST_STRING);
		}

		[Test]
		public void Execution()
		{
			string query = string.Format("test {0} {1} {2}", TEST_INT.ToString(), TEST_BOOL.ToString(), TEST_STRING);
			TestHelper.CheckCommand(m_Console, m_Message, query);
		}

		[Test]
		public void NamedArguments()
		{
			string query = string.Format("test str={2} {0} {1}", TEST_INT.ToString(), TEST_BOOL.ToString(), TEST_STRING);
			TestHelper.CheckCommand(m_Console, m_Message, query);
		}

		[Test]
		public void DefaultValues()
		{
			string query = string.Format("test {0} {1}", TEST_INT.ToString(), TEST_BOOL.ToString());
			TestHelper.CheckCommand(m_Console, m_Command.Command(TEST_INT, TEST_BOOL), query);
		}

		[Test]
		public void TooManyArguments()
		{
			string rawQuery = string.Format("test {0} {1} {2} {0}", TEST_INT.ToString(), TEST_BOOL.ToString(), TEST_STRING);
			Query query = m_Console.ParseQuery(rawQuery)[0];
			try
			{
				m_Command.Method.MapArguments(query);
				TestHelper.ExpectException<TooManyArgumentsException>();
			}
			catch(TooManyArgumentsException e)
			{
				Assert.AreEqual(3, e.Expected);
				Assert.AreEqual(4, e.Received);
			}
		}

		[Test]
		public void NotEnoughtArguments()
		{
			string rawQuery = string.Format("test {0}", TEST_INT.ToString());
			Query query = m_Console.ParseQuery(rawQuery)[0];
			try
			{
				m_Command.Method.MapArguments(query);
				TestHelper.ExpectException<NotEnoughtArgumentsException>();
			}
			catch(NotEnoughtArgumentsException e)
			{
				Assert.AreEqual(1, e.Parameters.Count);
				TestHelper.CheckParameter(e.Parameters[0], "toggle", typeof(bool), null);
			}
		}
	}
}
