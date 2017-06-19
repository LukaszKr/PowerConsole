using ProceduralLevel.PowerConsole.Logic;
using System.Reflection;

namespace ProceduralLevel.PowerConsole.Editor.Test
{
	public class TestCommand: AConsoleCommand
	{
		public TestCommand() : base("Test", "For test purpose")
		{
		}

		public Message Command(int numberValue, bool toggle, string defaultValue = "abc")
		{
			return new Message(EMessageType.Error, "Not Implemented");
		}

		#region Test only
		public MethodInfo GetIncorrectReturnTypeMethod()
		{
			return GetMethodInfo("IncorrectReturnType");
		}

		private int IncorrectReturnType()
		{
			return 0;
		}
		#endregion

		public override bool IsValid()
		{
			return true;
		}
	}
}
