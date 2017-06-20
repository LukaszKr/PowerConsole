using ProceduralLevel.Common.Parsing;
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


			return queries;
		}

		private Query ParseQuery()
		{
			Query query = null;

			bool quoted = false;
			Argument argument = null;
			Token token = null;
			string rawValue = "";

			while(HasTokens())
			{
				token = ConsumeToken();
				switch(token.Value)
				{
					case ParserConst.SPACE:
						AssertNamedArgumentValue(argument, token);
						argument = null;
						break;
					case ParserConst.QUOTE:
						quoted = !quoted;
						break;
					case ParserConst.SEPARATOR:
						AssertNamedArgumentValue(argument, token);
						if(query != null)
						{
							query.RawQuery = rawValue;
						}
						return query;
					case ParserConst.ASSIGN:
						if(argument == null)
						{
							throw new QueryParserException(EQueryError.NamedArgument_NoName, token);
						}
						argument.Name = argument.Value;
						argument.Value = null;
						break;
					default:
						if(query == null)
						{
							query = new Query(token.Value);
						}
						else
						{
							if(argument == null)
							{
								argument = new Argument();
								query.Arguments.Add(argument);
							}
							argument.Value = token.Value;
						}
						break;
				}
				rawValue += token.Value;
			}

			if(query != null)
			{
				query.RawQuery = rawValue;
			}
			AssertNamedArgumentValue(argument, token);
			if(quoted)
			{
				throw new QueryParserException(EQueryError.Quote_Mismatch, token);
			}
			return query;
		}

		private void AssertNamedArgumentValue(Argument param, Token token)
		{
			if(param != null && param.Value == null)
			{
				throw new QueryParserException(EQueryError.NamedArgument_NoValue, token);
			}
		}
	}
}
