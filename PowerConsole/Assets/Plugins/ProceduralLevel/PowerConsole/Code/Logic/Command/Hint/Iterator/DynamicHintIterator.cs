namespace ProceduralLevel.PowerConsole.Logic
{
	public class DynamicHintIterator<HintType>: AHintIterator
	{
		private string m_Current;

		public override string Current
		{
			get { return m_Current; }
		}

		private ADynamicHint<HintType> m_Hint;

		public DynamicHintIterator(Query query, Argument argument, ADynamicHint<HintType> hint)
			: base(query, argument, hint)
		{
			m_Hint = hint;
			m_Current = argument.Value;
		}

		public override bool MovePrev()
		{
			m_Current = m_Hint.PrevHint(m_Current);
			return !string.IsNullOrEmpty(m_Current);
		}

		public override bool MoveNext()
		{
			m_Current = m_Hint.NextHint(m_Current);
			return !string.IsNullOrEmpty(m_Current);
		}

		public override void Restart()
		{
		}
	}
}
