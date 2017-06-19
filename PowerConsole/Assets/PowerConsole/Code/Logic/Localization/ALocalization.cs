using System;

namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class ALocalization
	{
		public abstract string GetLocalizedKey(ELocalizationKey key);

		#region Command
		public string CommandNotFound(string commandName)
		{
			return string.Format(GetLocalizedKey(ELocalizationKey.Command_NotFound), commandName);
		}
		#endregion

		#region Parsing
		public string MissingValueParser(string rawValue, Type expectedType)
		{
			return string.Format(GetLocalizedKey(ELocalizationKey.Parsing_MissingParser), rawValue, expectedType.Name);
		}

		public string InvalidValueFormat(string rawValue, Type expectedType)
		{
			return string.Format(GetLocalizedKey(ELocalizationKey.Parsing_InvalidFormat), rawValue, expectedType.Name);
		}
		#endregion

		#region Query
		public string NamedArgumentNotFound(string argName, string argValue)
		{
			return string.Format(GetLocalizedKey(ELocalizationKey.Query_NamedArgumentNotFound), argName, argValue);
		}

		public string TooManyArguments(int received, int expected)
		{
			return string.Format(GetLocalizedKey(ELocalizationKey.Query_TooManyArguments), received, expected);
		}

		public string NotEnoughtArguments(int missingCount)
		{
			return string.Format(GetLocalizedKey(ELocalizationKey.Query_NotEnoughtArguments), missingCount);
		}
		#endregion
	}
}
