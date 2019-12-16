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
			while(HasTokens())
			{
				Query query = ParseQuery();
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

		private Query ParseQuery()
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
						bool isOption = false;
						int tokenColumn = token.Column;
						string tokenValue = token.Value;
						if(tokenValue.Length >= 1)
						{
							if(tokenValue[0] == ParserConst.OPTION)
							{
								if(tokenValue.Length >= 2)
								{
									isOption = !char.IsDigit(token.Value[1]);
									if(isOption)
									{
										tokenValue = tokenValue.Substring(1);
										tokenColumn ++;
									}
								}
							}
						}
						if(query == null)
						{
							Argument commandName = new Argument(true)
							{
								Name = ParserConst.NAME_ARGUMENT,
								Offset = tokenColumn,
								Value = tokenValue
							};
							query = new Query(commandName, isOption);
						}
						else
						{
							if(isOption)
							{
								query.RawQuery = rawValue;
								return query;
							}
							if(argument == null)
							{
								argument = new Argument();
								query.Arguments.Add(argument);
							}
							argument.Offset = tokenColumn;
							argument.Value = tokenValue;
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
			if(query != null)
			{
				if((token.IsSeparator && token.Value[0] == ParserConst.SPACE))
				{
					query.Arguments.Add(new Argument()
					{
						Offset = token.Column+token.Value.Length
					});
				}
			}
			return query;
		}
	}
}
