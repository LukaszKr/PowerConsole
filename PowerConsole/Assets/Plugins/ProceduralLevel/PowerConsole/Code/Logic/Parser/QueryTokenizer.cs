using ProceduralLevel.Tokenize;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class QueryTokenizer: ATokenizer
	{
		private readonly static char[] QUERY_DEFAULT = new char[]
		{
			ParserConst.QUOTE, ParserConst.OPTION,
			ParserConst.SEPARATOR, ParserConst.SPACE, ParserConst.ASSIGN,
		};

		private readonly static char[] QUERY_QUOTED = new char[]
		{
			ParserConst.QUOTE
		};

		private bool m_Quoted;

		public QueryTokenizer(bool autoTrim = false) : base(autoTrim)
		{
		}

		protected override void Clear()
		{
			base.Clear();
			m_Quoted = false;
		}

		protected override char[] GetSeparators(Token token)
		{
			switch(token.Value[0])
			{
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
					return GetSeparators();
			}
		}

		protected override char[] GetSeparators()
		{
			return QUERY_DEFAULT;
		}
	}
}
