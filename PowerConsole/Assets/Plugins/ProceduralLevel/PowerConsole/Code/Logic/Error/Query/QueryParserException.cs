using ProceduralLevel.Tokenize;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class QueryParserException: ParserException<EQueryError>
	{
		public QueryParserException(EQueryError errorCode, Token token) : base(errorCode, token)
		{
		}
	}
}
