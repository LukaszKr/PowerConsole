﻿using System.Reflection;

namespace ProceduralLevel.PowerConsole.Logic
{
	public static class Factory
	{
		public static CommandMethod CreateCommandMethod(MethodInfo info)
		{
			if(info.ReturnType != typeof(Message))
			{
				throw new IncorrectReturnTypeException(info, info.ReturnType);
			}
			CommandMethod method = new CommandMethod();
			CommandParameter[] parameters = ParseParameters(info);
			method.ClearParameters();
			for(int x = 0; x < parameters.Length; x++)
			{
				method.AddParameter(parameters[x]);
			}
			return method;
		}

		public static CommandParameter[] ParseParameters(MethodInfo method)
		{
			ParameterInfo[] info = method.GetParameters();
			CommandParameter[] parameters = new CommandParameter[info.Length];
			for(int x = 0; x < info.Length; x++)
			{
				CommandParameter parameter = new CommandParameter(info[x]);
				parameters[x] = parameter;
			}
			return parameters;
		}
	}
}