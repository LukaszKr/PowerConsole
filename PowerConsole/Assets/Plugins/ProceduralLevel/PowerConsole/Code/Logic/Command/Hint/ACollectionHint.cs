namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class ACollectionHint: AHint
	{
		private string[] m_Cache;
		private bool m_IsValid;

		public override string[] GetHints()
		{
			if(!m_IsValid)
			{
				m_IsValid = true;
				m_Cache = GetAllOptions();
			}
			return m_Cache;
		}

		protected abstract string[] GetAllOptions();

		public void InvalidateCache()
		{
			m_IsValid = false;
		}
	}
}
