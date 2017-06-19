using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Editor.Test
{
	public class ConsoleTest
	{
		private Console m_Console = new Console();

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
			TestHelper.CheckParam(parameters[0], "numberValue", typeof(int), null);
			TestHelper.CheckParam(parameters[1], "toggle", typeof(bool), null);
			TestHelper.CheckParam(parameters[2], "defaultValue", typeof(string), "abc");
		}
	}
}
