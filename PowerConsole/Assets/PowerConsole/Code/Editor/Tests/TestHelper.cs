using NUnit.Framework;
using ProceduralLevel.Common.Parsing;
using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.Editor.Test
{
	public static class TestHelper
	{
		public static void CheckParam(QueryParam param, string name, string value)
		{
			Assert.AreEqual(param.Name, name);
			Assert.AreEqual(param.Value, value);
		}

		public static void CheckToken(Token token, bool isSeparator, string value)
		{
			Assert.AreEqual(isSeparator, token.IsSeparator);
			Assert.AreEqual(value, token.Value);
		}

		public static void CheckException(QueryParser parser, string query, EQueryError errorType)
		{
			try
			{
				parser.Parse(query);
				parser.Flush();
				Assert.Fail(string.Format("Exception of type {0} wasn't caught.", errorType.ToString()));
			}
			catch(QueryParserException e)
			{
				Assert.AreEqual(errorType, e.ErrorCode);
			}
		}
	}
}
