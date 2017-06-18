using NUnit.Framework;
using ProceduralLevel.Common.Parsing;
using ProceduralLevel.GameConsole.Logic;
using System.Collections.Generic;

namespace ProceduralLevel.GameConsole.Editor.Test
{
	public class QueryTokenizerTest
	{
		private QueryTokenizer m_Tokenizer = new QueryTokenizer();

		[Test]
		public void TokenizeBasic()
		{
			m_Tokenizer.Tokenize("test param1 123 arg=value");
			List<Token> tokens = m_Tokenizer.Flush();
			Assert.AreEqual(9, tokens.Count);
			AssertToken(tokens[0], false, "test");
			AssertToken(tokens[1], true, " ");
			AssertToken(tokens[2], false, "param1");
			AssertToken(tokens[3], true, " ");
			AssertToken(tokens[4], false, "123");
			AssertToken(tokens[5], true, " ");
			AssertToken(tokens[6], false, "arg");
			AssertToken(tokens[7], true, "=");
			AssertToken(tokens[8], false, "value");
		}

		[Test]
		public void EscapedValues()
		{
			//only separator tokens can be escaped
			m_Tokenizer.Tokenize("\\\" \\escaped");
			List<Token> tokens = m_Tokenizer.Flush();
			Assert.AreEqual(2, tokens.Count);
			AssertToken(tokens[0], true, "\\");
			AssertToken(tokens[1], false, "\" \\escaped");
		}

		private void AssertToken(Token token, bool isSeparator, string value)
		{
			Assert.AreEqual(isSeparator, token.IsSeparator);
			Assert.AreEqual(value, token.Value);
		}
	}
}

