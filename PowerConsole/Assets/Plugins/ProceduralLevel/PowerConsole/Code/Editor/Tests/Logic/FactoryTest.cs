using NUnit.Framework;
using ProceduralLevel.PowerConsole.Logic;
using System;

namespace ProceduralLevel.PowerConsole.Editor.Test.Logic
{
    [Category("PowerConsole")]
    public class FactoryTest
	{
		[Test]
		public void IncorrectReturnTypeDetection()
		{
			TestCommand command = new TestCommand(null);
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

		[Test]
		public void NullMethodInfo()
		{
			try
			{
				Factory.CreateCommandMethod(null);
				TestHelper.ExpectException<ArgumentNullException>();
			}
			catch(ArgumentNullException e)
			{
				Assert.NotNull(e);
			}
		}
	}
}
