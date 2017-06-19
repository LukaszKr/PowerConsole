using ProceduralLevel.Common.Parsing;
using System;

namespace ProceduralLevel.GameConsole.Logic
{
	public class ParsingException: Exception
	{
		public readonly EParsingError ErrorCode;
		public readonly Token Token;

		public ParsingException(EParsingError errorCode, Token token) : base()
		{
			ErrorCode = errorCode;
			Token = token;
		}

		public override string ToString()
		{
			return string.Format("[ErrorCode: {0}, Token: {1}]", ErrorCode.ToString(), Token.ToString());
		}
	}
}
