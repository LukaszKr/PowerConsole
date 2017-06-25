using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class HistoryState: AConsoleState
	{
		public const int EXECUTION_HISTORY_LIMIT = 50;

		private List<string> m_ExecutionHistory = new List<string>(EXECUTION_HISTORY_LIMIT);

		public int Count
		{
			get { return m_ExecutionHistory.Count; }
		}

		public HistoryState(ConsoleInstance console) : base(console)
		{
		}

		public override void BindEvents()
		{
			throw new NotImplementedException();
		}

		#region Execution History
		public void Add(string entry)
		{
			int count = m_ExecutionHistory.Count;
			if(count == 0 || m_ExecutionHistory[count-1] != entry)
			{
				while(count+1 > EXECUTION_HISTORY_LIMIT)
				{
					m_ExecutionHistory.RemoveAt(0);
				}
				m_ExecutionHistory.Add(entry);
			}
		}

		public string Get(int index)
		{
			if(index >= 0 && index < m_ExecutionHistory.Count)
			{
				return m_ExecutionHistory[index];
			}
			return string.Empty;
		}

		public void ClearExecutionHistory()
		{
			m_ExecutionHistory.Clear();
		}
		#endregion
	}
}
