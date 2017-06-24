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
		public void MismatchedQuotes()
		{
			TestHelper.CheckException(m_Parser, "\"missing quote", EQueryError.Quote_Mismatch);
		}
	}
}
