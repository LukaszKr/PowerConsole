using System;
using System.Reflection;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class IncorrectReturnTypeException: ConsoleException
	{
		public readonly MethodInfo Method;
		public readonly Type ReturnedType;

		public IncorrectReturnTypeException(MethodInfo method, Type returnedType)
		{
			Method = method;
			ReturnedType = returnedType;
		}

		public override string ToString()
		{
			return string.Format("[Method: {0}, ReturnedType: {1}, Expected: {2}]", Method.Name, ReturnedType.Name, typeof(Message).Name);
		}
	}
}
