using ProceduralLevel.Common.Parsing;
using System.Collections.Generic;

namespace ProceduralLevel.GameConsole.Logic
{
	public class QueryParser: AParser<List<Query>>
	{
		public QueryParser(ATokenizer tokenizer) : base(tokenizer)
		{
		}

		protected override void Reset()
		{
			base.Reset();
		}

		protected override List<Query> Parse()
		{
			List<Query> queries = new List<Query>();

			return queries;
		}
	}
}
