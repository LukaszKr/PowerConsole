using System;
using System.Reflection;
using System.Text;

namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class AConsoleCommand
	{
		public const string DEFAULT_COMMAND_NAME = "Command";

		public readonly ConsoleInstance Console;
		public readonly string Name;
		public readonly string Description;
		public CommandMethod Method { get; private set; }

		public AConsoleCommand(ConsoleInstance console, string name, string description)
		{
			Console = console;
			Name = name.ToLowerInvariant();
			Description = description;
		}

		public void ParseMethod()
		{
			Method = Factory.CreateCommandMethod(GetCommandMethod());
		}

		public virtual AHint GetHintFor(HintManager manager, int parameterIndex)
		{
			CommandParameter parameter = Method.GetParameter(parameterIndex);
			if(parameter != null)
			{
				return manager.GetHint(parameter.Type);
			}
			return null;
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

		public Message Execute(object[] values)
		{
			object rawResult = Method.Command.Invoke(this, values);
			return rawResult as Message;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(Name).Append("(");
			if(Method != null)
			{
				for(int x = 0; x < Method.ParameterCount; x++)
				{
					CommandParameter parameter = Method.GetParameter(x);
					if(x > 0)
					{
						sb.Append(", ");
					}
					sb.Append(parameter.Name);
					if(parameter.HasDefault)
					{
						sb.Append("=").Append(parameter.DefaultValue.ToString());
					}
				}
			}
			sb.Append(")");
			return sb.ToString();
		}
	}
}
