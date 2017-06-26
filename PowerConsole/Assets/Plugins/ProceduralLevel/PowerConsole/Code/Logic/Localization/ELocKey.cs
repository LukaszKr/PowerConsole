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

		UIInputSubmit,


		CmdHelpName,
		CmdHelpDesc,
		CmdClearName,
		CmdClearDesc,
		CmdPrintName,
		CmdPrintDesc,
		CmdMacroName,
		CmdMacroDesc,
		CmdClearHistoryName,
		CmdClearHistoryDesc,

		ResMacroModeUnknown,
		ResMacroAlreadyRecording,
		ResMacroRecording,
		ResMacroNameEmpty,
		ResMacroRemoved,
		ResMacroNotRemoved,
		ResClearHistory,
	}
}
