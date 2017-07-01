namespace ProceduralLevel.PowerConsole.Logic
{
	public enum ELocKey
	{
		LogicCommandNotFound,
		LogicParsingMissingParser,
		LogicParsingInvalidFormat,
		LogicQueryNamedArgumentNotFound,
		LogicQueryTooManyArguments,
		LogicQueryNotEnoughtArguments,
		LogicConsoleLocked,
		LogicOptionWithoutCommand,
		LogicOptionInvalid,
		LogicParserError,

		UIInputSubmit,

		CmdClearName,
		CmdClearDesc,
		CmdPrintName,
		CmdPrintDesc,
		CmdMacroName,
		CmdMacroDesc,
		CmdMacroPlayerDesc,
		CmdClearHistoryName,
		CmdClearHistoryDesc,
		CmdListName,
		CmdListDesc,

		OptRepeatName,
		OptRepeatDesc,

		ResMacroModeUnknown,
		ResMacroAlreadyRecording,
		ResMacroRecording,
		ResMacroNameEmpty,
		ResMacroRemoved,
		ResMacroNotRemoved,
		ResMacroNotRecording,
		ResMacroSaved,
		ResMacroList,

		ResClearHistory,
	}
}
