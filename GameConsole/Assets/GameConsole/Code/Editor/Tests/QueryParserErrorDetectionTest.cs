using NUnit.Framework;
using ProceduralLevel.GameConsole.Logic;

namespace ProceduralLevel.GameConsole.Editor.Test
{
	public class QueryParserErrorDetectionTest
	{
		private QueryParser m_Parser = new QueryParser();

		[Test]
		public void NamedWithoutName()
		{
			TestHelper.CheckException(m_Parser, "test =value", EParsingError.NamedParam_NoName);
		}


		[Test]
		public void NamedWithoutValue()
		{
			TestHelper.CheckException(m_Parser, "test key=", EParsingError.NamedParam_NoValue);
			TestHelper.CheckException(m_Parser, "test key= value", EParsingError.NamedParam_NoValue);
			TestHelper.CheckException(m_Parser, "test key=;value", EParsingError.NamedParam_NoValue);

		}
	}
}
