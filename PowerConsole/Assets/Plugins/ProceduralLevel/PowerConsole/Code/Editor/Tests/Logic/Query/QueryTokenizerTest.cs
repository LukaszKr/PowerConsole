using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;
using ProceduralLevel.Tokenize;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Editor.Test.Logic.Queries
{
    [Category("PowerConsole")]
    public class QueryTokenizerTest
	{
		private QueryTokenizer m_Tokenizer = new QueryTokenizer();

		[Test]
		public void TokenizeBasic()
		{
			m_Tokenizer.Tokenize("test param1 123 arg=value");
			List<Token> tokens = m_Tokenizer.Flush();
			Assert.AreEqual(9, tokens.Count);
			TestHelper.CheckToken(tokens[0], false, "test");
			TestHelper.CheckToken(tokens[1], true, " ");
			TestHelper.CheckToken(tokens[2], false, "param1");
			TestHelper.CheckToken(tokens[3], true, " ");
			TestHelper.CheckToken(tokens[4], false, "123");
			TestHelper.CheckToken(tokens[5], true, " ");
			TestHelper.CheckToken(tokens[6], false, "arg");
			TestHelper.CheckToken(tokens[7], true, "=");
			TestHelper.CheckToken(tokens[8], false, "value");
		}

		[Test]
		public void EscapedValues()
		{
			//only separator tokens can be escaped
			m_Tokenizer.Tokenize("\\\" \\escaped \\\"test\\\"");
			List<Token> tokens = m_Tokenizer.Flush();
			Assert.AreEqual(5, tokens.Count);
			TestHelper.CheckToken(tokens[0], false, "\"");
			TestHelper.CheckToken(tokens[1], true, " ");
			TestHelper.CheckToken(tokens[2], false, "escaped");
			TestHelper.CheckToken(tokens[3], true, " ");
			TestHelper.CheckToken(tokens[4], false, "\"test\"");
		}

		[Test]
		public void QuoteTest()
		{
			m_Tokenizer.Tokenize("\"quoted\"");
			List<Token> tokens = m_Tokenizer.Flush();
			Assert.AreEqual(3, tokens.Count);
			TestHelper.CheckToken(tokens[0], true, "\"");
			TestHelper.CheckToken(tokens[1], false, "quoted");
			TestHelper.CheckToken(tokens[2], true, "\"");
		}

		[Test]
		public void IncompleteQuoteTest()
		{
			m_Tokenizer.Tokenize("\"partially quoted");
			List<Token> tokens = m_Tokenizer.Flush();
			Assert.AreEqual(2, tokens.Count);
			TestHelper.CheckToken(tokens[0], true, "\"");
			TestHelper.CheckToken(tokens[1], false, "partially quoted");
		}
	}
}

