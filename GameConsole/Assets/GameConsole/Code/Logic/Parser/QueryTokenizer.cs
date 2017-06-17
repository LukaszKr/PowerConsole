using ProceduralLevel.Common.Parsing;

namespace ProceduralLevel.GameConsole.Logic
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

		private bool m_Quoted;
		private bool m_Escaped;

		public QueryTokenizer(bool autoTrim = false) : base(autoTrim)
		{
		}

		protected override void Reset()
		{
			base.Reset();
			m_Quoted = false;
			m_Escaped = false;
		}

		protected override string[] GetSeparators(Token token)
		{
			if(m_Escaped)
			{
				m_Escaped = false;
				if(token.IsSeparator)
				{
					return GetDefaultSeparators();
				}
			}
			switch(token.Value)
			{
				case ParserConst.ESCAPE:
					m_Escaped = true;
					return GetDefaultSeparators();
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
