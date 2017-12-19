using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.Editor.Test.Logic
{
    [Category("PowerConsole")]
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
	}
}
