using System;
using System.Reflection;

namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class AConsoleCommand
	{
		public const string DEFAULT_COMMAND_NAME = "Command";

		public virtual MethodInfo GetCommandMethod()
		{
			return GetMethodInfo(DEFAULT_COMMAND_NAME);
		}

		protected MethodInfo GetMethodInfo(string methodName)
		{
			Type type = GetType();
			return type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		}

		public abstract bool IsValid();

		public Message Execute()
		{
			return null;
		}
	}
}
