using ProceduralLevel.Common.Parsing;
using System.Collections.Generic;

namespace ProceduralLevel.GameConsole.Logic
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
				queries.Add(query);
			}


			return queries;
		}

		private Query ParseQuery()
		{
			Query query = new Query();

			QueryParam param = null;

			//todo: error handling
			while(HasTokens())
			{
				Token token = ConsumeToken();
				switch(token.Value)
				{
					case ParserConst.SPACE:
						if(param != null && param.Value == null)
						{
							throw new ParsingException(EParsingError.NamedParam_NoValue, token);
						}
						param = null;
						break;
					case ParserConst.SEPARATOR:
						if(param != null && param.Value == null)
						{
							throw new ParsingException(EParsingError.NamedParam_NoValue, token);
						}
						return query;
					case ParserConst.ASSIGN:
						if(param == null)
						{
							throw new ParsingException(EParsingError.NamedParam_NoName, token);
						}
						param.Name = param.Value;
						param.Value = null;
						break;
					default:
						if(param == null)
						{
							param = new QueryParam();
							query.Params.Add(param);
						}
						param.Value = token.Value;
						break;
				}
			}

			return query;
		}
	}
}
