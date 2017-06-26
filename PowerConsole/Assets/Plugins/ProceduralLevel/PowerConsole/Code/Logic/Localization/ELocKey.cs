namespace ProceduralLevel.PowerConsole.Logic
{
	public enum ELocKey
	{
		CommandNotFound,
		ParsingMissingParser,
		ParsingInvalidFormat,
		QueryNamedArgumentNotFound,
		QueryTooManyArguments,
		QueryNotEnoughtArguments,
		InputSubmit,
		ConsoleLocked,

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
