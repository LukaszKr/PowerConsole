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

		UIInputSubmit,

		CmdHelpName,
		CmdHelpDesc,
		CmdClearName,
		CmdClearDesc,
		CmdPrintName,
		CmdPrintDesc,
		CmdMacroName,
		CmdMacroDesc,
		CmdMacroPlayerDesc,
		CmdClearHistoryName,
		CmdClearHistoryDesc,

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

		ResHelpInput,
		ResHelpMacro,
		ResHelpUnknownTopic,
	}
}
