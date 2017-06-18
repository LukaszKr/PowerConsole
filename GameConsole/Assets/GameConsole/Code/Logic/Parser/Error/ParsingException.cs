using ProceduralLevel.Common.Parsing;
using System;

namespace ProceduralLevel.GameConsole.Logic
{
	public class ParsingException: Exception
	{
		public readonly EParsingError Error;
		public readonly Token Token;

		public ParsingException(EParsingError error, Token token) : base()
		{
			Error = error;
			Token = token;
		}
	}
}
