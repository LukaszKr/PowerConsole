using System;
using System.Collections.Generic;
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
		public bool IsOption {  get; private set; }

		public virtual bool ObeyLock { get { return true; } }

		private HashSet<Type> m_ValidOptions = new HashSet<Type>();

		public AConsoleCommand(ConsoleInstance console, string name, string description, bool isOption = false)
		{
			IsValid = true;
			IsOption = isOption;
			Console = console;
			Name = name.ToLowerInvariant();
			Description = description;
		}

		protected AConsoleCommand(ConsoleInstance console, ELocKey name, ELocKey description, bool isOption = false)
			: this(console, console.Localization.Get(name), console.Localization.Get(description), isOption)
		{
		}

		public void SetValid(bool valid)
		{
			if(IsValid != valid)
			{
				IsValid = valid;
				if(IsOption)
				{
					Console.OptionHint.InvalidateCache();
				}
				else
				{
					Console.NameHint.InvalidateCache();
				}
			}
		}

		public void ParseMethod()
		{
			Method = Factory.CreateCommandMethod(GetCommandMethod());
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

		#region Options
		public bool IsOptionValid<OptionType>() where OptionType: AConsoleCommand
		{
			return m_ValidOptions.Contains(typeof(OptionType));
		}

		public bool IsOptionValid(Type type)
		{
			return m_ValidOptions.Contains(type);
		}

		public bool AddValidOption<OptionType>() where OptionType: AConsoleCommand
		{
			return AddValidOption(typeof(OptionType));
		}

		public bool AddValidOption(Type type)
		{
			if(!IsOptionValid(type))
			{
				m_ValidOptions.Add(type);
				return true;
			}
			return false;
		}
		#endregion

		#region Getters
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
		#endregion


		#region Result Creation
		protected Message CreateMessage(EMessageType message, ELocKey key)
		{
			return new Message(message, Console.Localization.Get(key));
		}

		protected Message CreateMessage(EMessageType message, ELocKey key, params object[] args)
		{
			return new Message(message, Console.Localization.Get(key, args));
		}
		#endregion

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
