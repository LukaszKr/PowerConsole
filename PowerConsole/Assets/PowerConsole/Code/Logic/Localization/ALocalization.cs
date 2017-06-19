using System;

namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class ALocalization
	{
		public abstract string GetLocalizedKey(ELocalizationKey key);

		public string CommandNotFound(string commandName)
		{
			return string.Format(GetLocalizedKey(ELocalizationKey.Command_NotFound), commandName);
		}

		public string MissingValueParser(string rawValue, Type expectedType)
		{
			return string.Format(GetLocalizedKey(ELocalizationKey.Parsing_MissingParser), rawValue, expectedType.Name);
		}

		public string InvalidValueFormat(string rawValue, Type expectedType)
		{
			return string.Format(GetLocalizedKey(ELocalizationKey.Parsing_InvalidFormat), rawValue, expectedType.Name);
		}
	}
}
