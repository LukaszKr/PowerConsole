namespace ProceduralLevel.PowerConsole.Logic
{
	public enum ELocKey
	{
		CommandNotFound = 0,

		ParsingMissingParser = 10,
		ParsingInvalidFormat = 11,

		QueryNamedArgumentNotFound = 20,
		QueryTooManyArguments = 21,
		QueryNotEnoughtArguments = 22,

		InputSubmit = 30,

		ConsoleLocked = 40,

		CmdHelpName = 100,
		CmdHelpDesc = 101,
		CmdClearName = 102,
		CmdClearDesc = 103,
		CmdMacroMode = 104,
		CmdMacroAlreadyRecording = 105,
		CmdMacroRecording = 106,
		CmdMacroNameEmpty = 107,
	}
}
