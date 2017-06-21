using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.Editor.Test.Logic
{
	public class FactoryTest
	{
		[Test]
		public void IncorrectReturnTypeDetection()
		{
			TestCommand command = new TestCommand();
			try
			{
				Factory.CreateCommandMethod(command.GetIncorrectReturnTypeMethod());
				TestHelper.ExpectException<IncorrectReturnTypeException>();
			}
			catch(IncorrectReturnTypeException e)
			{
				Assert.AreEqual(typeof(int), e.ReturnedType);
			}
		}
	}
}
