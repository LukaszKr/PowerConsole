using ProceduralLevel.Tokenize;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class QueryParser: AParser<List<Query>>
	{
		public QueryParser() : base(new QueryTokenizer())
		{
		}

		protected override void Reset()
		{
			base.Reset();
		}

		protected override List<Query> Parse()
		{
			List<Query> queries = new List<Query>();
			bool isOption;
			while(HasTokens())
			{
				Token token = PeekToken();
				isOption = (token.IsSeparator && token.Value[0] == ParserConst.OPTION);
				if(isOption)
				{
					if(queries.Count == 0)
					{
						throw new QueryParserException(EQueryError.OptionWithoutCommand, token);
					}
					ConsumeToken();
				}
				Query query = ParseQuery(isOption);
				if(query != null)
				{
					queries.Add(query);
				}
			}

			if(queries.Count == 0)
			{
				Argument commandName = new Argument(true)
				{
					Name = ParserConst.NAME_ARGUMENT,
					Offset = 0,
					Value = string.Empty
				};
				queries.Add(new Query(commandName, false));
			}

			return queries;
		}

		private Query ParseQuery(bool isOption)
		{
			Query query = null;

			bool quoted = false;
			Argument argument = null;
			Token token = null;
			string rawValue = "";
			Token prevToken = null;

			while(HasTokens())
			{
				prevToken = token;
				token = PeekToken();
				
				switch(token.Value[0])
				{
					case ParserConst.SPACE:
						if(prevToken != null && prevToken.Value == token.Value)
						{
							argument = new Argument()
							{ 
								Offset = token.Column
							};
							query.Arguments.Add(argument);
						}
						argument = null;
						ConsumeToken();
						break;
					case ParserConst.QUOTE:
						quoted = !quoted;
						ConsumeToken();
						break;
					case ParserConst.OPTION: //do not consume token
						if(query != null)
						{
							query.RawQuery = rawValue;
						}
						return query;
					case ParserConst.SEPARATOR:
						if(query != null)
						{
							query.RawQuery = rawValue;
						}
						ConsumeToken();
						return query;
					case ParserConst.ASSIGN:
						if(argument == null)
						{
							throw new QueryParserException(EQueryError.NamedArgumentNoName, token);
						}
						argument.Name = argument.Value;
						argument.Value = string.Empty;
						argument.Offset = token.Column+token.Value.Length;
						ConsumeToken();
						break;
					default:
						if(query == null)
						{
							Argument commandName = new Argument(true)
							{
								Name = ParserConst.NAME_ARGUMENT,
								Offset = token.Column,
								Value = token.Value
							};
							query = new Query(commandName, isOption);
						}
						else
						{
							if(argument == null)
							{
								argument = new Argument();
								query.Arguments.Add(argument);
							}
							argument.Offset = token.Column;
							argument.Value = token.Value;
						}
						ConsumeToken();
						break;
				}
				rawValue += token.Value;
			}

			if(query != null)
			{
				query.RawQuery = rawValue;
			}
			if(quoted)
			{
				throw new QueryParserException(EQueryError.QuoteMismatch, token);
			}
			if(query != null && token.IsSeparator && token.Value[0] == ParserConst.SPACE)
			{
				query.Arguments.Add(new Argument() 
				{ 
					Offset = token.Column+token.Value.Length
				});
			}
			return query;
		}
	}
}
