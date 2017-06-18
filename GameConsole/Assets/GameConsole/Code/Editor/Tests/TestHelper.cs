using NUnit.Framework;
using ProceduralLevel.Common.Parsing;
using ProceduralLevel.GameConsole.Logic;

namespace ProceduralLevel.GameConsole.Editor.Test
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
	}
}
