using ProceduralLevel.Tokenize;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class QueryParser: AParser<List<Query>>
	{
		private EQueryParseMode m_ParseMode = EQueryParseMode.Argument;

		public QueryParser() : base(new QueryTokenizer())
		{
			m_ParseMode = EQueryParseMode.Argument;
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
					case ParserConst.OPTION: //do not consume token
						ConsumeToken();
						if(HasTokens())
						{
							Token nextToken = PeekToken();
							if(string.IsNullOrEmpty(nextToken.Value) || char.IsDigit(nextToken.Value[0]))
							{
								m_ParseMode = EQueryParseMode.NegativeNumber;
							}
							else
							{
								m_ParseMode = EQueryParseMode.Option;
								if(query != null)
								{
									query.RawQuery = rawValue;
								}
								else
								{
									throw new QueryParserException(EQueryError.OptionWithoutCommand, nextToken);
								}
								return query;
							}
						}
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
						if(query == null)
						{
							Argument commandName = new Argument(true)
							{
								Name = ParserConst.NAME_ARGUMENT,
								Offset = token.Column,
								Value = token.Value
							};
							query = new Query(commandName, m_ParseMode == EQueryParseMode.Option);
						}
						else
						{
							if(argument == null)
							{
								argument = new Argument();
								query.Arguments.Add(argument);
							}
							string argumentValue;
							int offsetColumn = token.Column;
							if(m_ParseMode == EQueryParseMode.NegativeNumber)
							{
								argumentValue = ParserConst.OPTION+token.Value;
								m_ParseMode = EQueryParseMode.Argument;
								offsetColumn -= 1;
							}
							else
							{
								argumentValue = token.Value;
							}

							argument.Offset = offsetColumn;
							argument.Value = argumentValue;
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
				if((token.IsSeparator && token.Value[0] == ParserConst.SPACE) || m_ParseMode == EQueryParseMode.NegativeNumber)
				{
					query.Arguments.Add(new Argument() 
					{ 
						Offset = token.Column+token.Value.Length
					});
				}
			}
			m_ParseMode = EQueryParseMode.Argument;
			return query;
		}
	}
}
