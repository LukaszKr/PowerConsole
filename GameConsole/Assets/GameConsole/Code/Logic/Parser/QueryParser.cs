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
						param = null;
						break;
					case ParserConst.SEPARATOR:
						return query;
					case ParserConst.ASSIGN:
						param.Name = param.Value;
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
