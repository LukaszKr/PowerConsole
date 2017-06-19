using ProceduralLevel.Common.Parsing;
using System;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class QueryParserException: Exception
	{
		public readonly EQueryError ErrorCode;
		public readonly Token Token;

		public QueryParserException(EQueryError errorCode, Token token) : base()
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
