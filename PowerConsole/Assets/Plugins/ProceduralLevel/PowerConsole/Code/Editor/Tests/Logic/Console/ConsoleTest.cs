using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Editor.Test.Logic.Console
{
    [Category("PowerConsole")]
    public class ConsoleTest
	{
		private ConsoleInstance m_Console = new ConsoleInstance(new LocalizationManager(), null);


		public ConsoleTest()
		{
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
			TestCommand command = new TestCommand(m_Console);
			command.ParseMethod();
			CommandMethod method = command.Method;
			List<CommandParameter> parameters = method.CopyParameters();
			
			Assert.AreEqual(3, method.ParameterCount);
			TestHelper.CheckParameter(parameters[0], "numbervalue", typeof(int), null);
			TestHelper.CheckParameter(parameters[1], "toggle", typeof(bool), null);
			TestHelper.CheckParameter(parameters[2], "str", typeof(string), "abc");
		}
	}
}
