using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.Editor.Test.Logic.Queries
{
    [Category("PowerConsole")]
    public class QueryParserErrorDetectionTest
	{
		private QueryParser m_Parser = new QueryParser();

		[Test]
		public void NamedWithoutName()
		{
			TestHelper.CheckException(m_Parser, "test =value", EQueryError.NamedArgumentNoName);
		}

		[Test]
		public void MismatchedQuotes()
		{
			TestHelper.CheckException(m_Parser, "\"missing quote", EQueryError.QuoteMismatch);
		}

		[Test]
		public void OptionRequiresACommand()
		{
			TestHelper.CheckException(m_Parser, "-option 123", EQueryError.OptionWithoutCommand);
		}
	}
}
