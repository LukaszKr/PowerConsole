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
		public bool IsValid { get; private set; }

		public virtual bool ObeyLock { get { return true; } }
		protected LocalizationManager Localization { get { return Console.Localization; } }

		public AConsoleCommand(ConsoleInstance console, string name, string description)
		{
			IsValid = true;
			Console = console;
			Name = name.ToLowerInvariant();
			Description = description;
		}

		public AConsoleCommand(ConsoleInstance console, ELocKey name, ELocKey description)
			: this(console, console.Localization.Get(name), console.Localization.Get(description))
		{
		}

		public void SetValid(bool valid)
		{
			if(IsValid != valid)
			{
				IsValid = valid;
				Console.NameHint.InvalidateCache();
			}
		}

		public void ParseMethod()
		{
			Method = Factory.CreateCommandMethod(GetCommandMethod());
		}

		protected Message CreateMessage(EMessageType message, ELocKey key)
		{
			return new Message(message, Localization.Get(key));
		}

		protected Message CreateMessage(EMessageType message, ELocKey key, params object[] args)
		{
			return new Message(message, Localization.Get(key, args));
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


		public Message Execute(object[] values)
		{
			if(!IsValid)
			{
				return new Message(EMessageType.Error, "Command is marked as invalid for current state");
			}
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
