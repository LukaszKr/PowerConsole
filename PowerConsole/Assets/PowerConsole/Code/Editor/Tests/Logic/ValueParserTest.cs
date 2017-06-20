using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.Editor.Test.Logic
{
	public class ValueParserTest
	{
		private ValueParser m_Parser = new ValueParser();

		[Test]
		public void ValueParsingError()
		{
			try
			{
				m_Parser.Parse<int>("a123");
				TestHelper.ExpectException<MissingValueParserException>();
			}
			catch(InvalidValueFormatException e)
			{
				Assert.AreEqual(typeof(int), e.ExpectedType);
				Assert.AreEqual("a123", e.RawValue);
			}
		}

		private enum TestEnum { Value };

		[Test]
		public void MissingParserError()
		{
			try
			{
				m_Parser.Parse<TestEnum>("Value");
				TestHelper.ExpectException<MissingValueParserException>();
			}
			catch(MissingValueParserException e)
			{
				Assert.AreEqual(typeof(TestEnum), e.ExpectedType);
				Assert.AreEqual("Value", e.RawValue);
			}
		}
	}
}
