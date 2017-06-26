using System;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class LocalizationManager
	{
		public string Get(ELocKey key)
		{
			return key.ToString();
		}

		public string Get(ELocKey key, params object[] args)
		{
			return string.Format(Get(key), args);
		}

		#region Macro
		public string MacroModeNotSupported(EMacroMode mode)
		{
			return Get(ELocKey.CmdMacroMode, mode);
		}
		#endregion

		#region Command
		public string CommandNotFound(string commandName)
		{
			return Get(ELocKey.CommandNotFound, commandName);
		}
		#endregion

		#region Parsing
		public string MissingValueParser(string rawValue, Type expectedType)
		{
			return Get(ELocKey.ParsingMissingParser, rawValue, expectedType);
		}

		public string InvalidValueFormat(string rawValue, Type expectedType)
		{
			return Get(ELocKey.ParsingInvalidFormat, rawValue, expectedType);
		}
		#endregion

		#region Query
		public string NamedArgumentNotFound(string argName, string argValue)
		{
			return Get(ELocKey.QueryNamedArgumentNotFound, argName, argValue);
		}

		public string TooManyArguments(int received, int expected)
		{
			return Get(ELocKey.QueryTooManyArguments, received, expected);
		}

		public string NotEnoughtArguments(int missingCount)
		{
			return Get(ELocKey.QueryNotEnoughtArguments, missingCount);
		}
		#endregion
	}
}
