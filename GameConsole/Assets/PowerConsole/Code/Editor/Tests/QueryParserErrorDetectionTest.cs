using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.Editor.Test
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

		[Test]
		public void MismatchedQuotes()
		{
			TestHelper.CheckException(m_Parser, "\"missing quote", EParsingError.Quote_Mismatch);
		}
	}
}
