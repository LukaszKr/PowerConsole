using System;
using System.Reflection;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class CommandParameter
	{
		public readonly string Name;
		public readonly Type Type;
		public readonly object DefaultValue;

		public bool HasDefault { get { return DefaultValue != null; } }

		public CommandParameter(ParameterInfo parameterInfo)
		{
			Name = parameterInfo.Name.ToLowerInvariant();
			Type = parameterInfo.ParameterType;
			bool hasDefaultValue = (parameterInfo.Attributes & ParameterAttributes.HasDefault) != 0;
			DefaultValue = (hasDefaultValue? parameterInfo.DefaultValue: null);
		}

		public CommandParameter(string name, Type type, object defaultValue = null)
		{
			Name = name;
			Type = type;
			DefaultValue = defaultValue;
		}

		public override string ToString()
		{
			if(HasDefault)
			{
				return string.Format("[{0} {1}={2}]", Name, Type.Name, DefaultValue.ToString());
			}
			else
			{
				return string.Format("[{0} {1}]", Name, Type.Name);
			}
		}
	}
}
