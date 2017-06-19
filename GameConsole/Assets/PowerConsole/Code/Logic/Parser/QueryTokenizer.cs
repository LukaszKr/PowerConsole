using ProceduralLevel.Common.Parsing;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class QueryTokenizer: ATokenizer
	{
		private readonly static string[] QUERY_DEFAULT = new string[]
		{
			ParserConst.ESCAPE, ParserConst.QUOTE,
			ParserConst.SEPARATOR, ParserConst.SPACE, ParserConst.ASSIGN
		};

		private readonly static string[] QUERY_QUOTED = new string[]
		{
			ParserConst.QUOTE
		};

		private readonly static string[] QUERY_ESCAPED = new string [] { };

		private bool m_Quoted;

		public QueryTokenizer(bool autoTrim = false) : base(autoTrim)
		{
		}

		protected override void Reset()
		{
			base.Reset();
			m_Quoted = false;
		}

		protected override string[] GetSeparators(Token token)
		{
			switch(token.Value)
			{
				case ParserConst.ESCAPE:
					return QUERY_ESCAPED;
				case ParserConst.QUOTE:
					m_Quoted = !m_Quoted;
					if(m_Quoted)
					{
						return QUERY_QUOTED;
					}
					else
					{
						return QUERY_DEFAULT;
					}
				default:
					return GetDefaultSeparators();
			}
		}

		protected override string[] GetDefaultSeparators()
		{
			return QUERY_DEFAULT;
		}
	}
}
