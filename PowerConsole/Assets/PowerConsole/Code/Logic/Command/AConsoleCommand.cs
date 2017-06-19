using System;
using System.Reflection;

namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class AConsoleCommand
	{
		public const string DEFAULT_COMMAND_NAME = "Command";

		public readonly string Name;
		public readonly string Description;
		public CommandMethod Method { get; private set; }

		public AConsoleCommand(string name, string description)
		{
			Name = name.ToLowerInvariant();
			Description = description;
		}

		public void ParseMethod()
		{
			Method = Factory.CreateCommandMethod(GetCommandMethod());
		}

		public virtual MethodInfo GetCommandMethod()
		{
			MethodInfo info = GetMethodInfo(DEFAULT_COMMAND_NAME);
			return info;
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
