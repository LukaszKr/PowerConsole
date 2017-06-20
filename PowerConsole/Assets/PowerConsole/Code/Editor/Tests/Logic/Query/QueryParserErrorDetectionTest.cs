using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.Editor.Test.Logic.Queries
{
	public class QueryParserErrorDetectionTest
	{
		private QueryParser m_Parser = new QueryParser();

		[Test]
		public void NamedWithoutName()
		{
			TestHelper.CheckException(m_Parser, "test =value", EQueryError.NamedArgument_NoName);
		}


		[Test]
		public void NamedWithoutValue()
		{
			TestHelper.CheckException(m_Parser, "test key=", EQueryError.NamedArgument_NoValue);
			TestHelper.CheckException(m_Parser, "test key= value", EQueryError.NamedArgument_NoValue);
			TestHelper.CheckException(m_Parser, "test key=;value", EQueryError.NamedArgument_NoValue);
		}

		[Test]
		public void MismatchedQuotes()
		{
			TestHelper.CheckException(m_Parser, "\"missing quote", EQueryError.Quote_Mismatch);
		}
	}
}
