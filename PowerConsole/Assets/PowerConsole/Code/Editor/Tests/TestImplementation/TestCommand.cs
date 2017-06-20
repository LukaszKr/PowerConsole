﻿using ProceduralLevel.PowerConsole.Logic;
using System.Reflection;

namespace ProceduralLevel.PowerConsole.Editor.Test
{
	public class TestCommand: AConsoleCommand
	{
		public TestCommand() : base("Test", "For test purpose")
		{
		}

		public Message Command(int numberValue, bool toggle, string str = "abc")
		{
			return new Message(EMessageType.Success, string.Format("{0}:{1}:{2}", numberValue.ToString(), toggle.ToString(), str.ToString()));
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
