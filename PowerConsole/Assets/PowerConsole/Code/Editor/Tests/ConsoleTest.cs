using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Editor.Test
{
	public class ConsoleTest
	{
		private Console m_Console = new Console(new DefaultLocalization());
		private TestCommand m_Command;

		public ConsoleTest()
		{
			m_Command = new TestCommand();
			m_Console.AddCommand(m_Command);
		}

		[Test]
		public void BasicParseQuery()
		{
			List<Query> queries = m_Console.ParseQuery("test; test");
			Assert.AreEqual(2, queries.Count); //proper query parsing is tested in QueryParserTest class
		}

		[Test]
		public void MethodParsing()
		{
			TestCommand command = new TestCommand();
			command.ParseMethod();
			CommandMethod method = command.Method;
			List<CommandParameter> parameters = method.CopyParameters();
			
			Assert.AreEqual(3, method.ParameterCount);
			TestHelper.CheckParameter(parameters[0], "numbervalue", typeof(int), null);
			TestHelper.CheckParameter(parameters[1], "toggle", typeof(bool), null);
			TestHelper.CheckParameter(parameters[2], "defaultvalue", typeof(string), "abc");
		}

		[Test]
		public void Execution()
		{
			string query = "test 123 true abcd";
			Message[] expected = new Message[]
			{
				new Message(EMessageType.Execution, query),
				m_Command.Command(123, true, "abcd")
			};

			int current = 0;
			System.Action<Message> onMessage = (Message message) =>
			{
				TestHelper.CheckMessage(message, expected[current].Result, expected[current].Value);
				current += 1;
			};
			m_Console.OnMessage.AddListener(onMessage);
			m_Console.Execute(query);
			m_Console.OnMessage.RemoveAllListeners();
		}

	}
}
