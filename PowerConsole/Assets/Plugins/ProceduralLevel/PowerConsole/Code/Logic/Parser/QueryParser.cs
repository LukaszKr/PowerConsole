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
						argument = null;
						break;
					case ParserConst.QUOTE:
						quoted = !quoted;
						break;
					case ParserConst.SEPARATOR:
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
						argument.Value = string.Empty;
						argument.Offset = token.Position+token.Value.Length;
						break;
					default:
						if(query == null)
						{
							Argument commandName = new Argument(true)
							{
								Name = ParserConst.NAME_ARGUMENT,
								Offset = token.Position,
								Value = token.Value
							};
							query = new Query(commandName);
						}
						else
						{
							if(argument == null)
							{
								argument = new Argument();
								query.Arguments.Add(argument);
							}
							argument.Offset = token.Position;
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
			if(quoted)
			{
				throw new QueryParserException(EQueryError.Quote_Mismatch, token);
			}
			if(token.IsSeparator && token.Value == ParserConst.SPACE)
			{
				query.Arguments.Add(new Argument() 
				{ 
					Offset = token.Position+token.Value.Length
				});
			}
			return query;
		}
	}
}
