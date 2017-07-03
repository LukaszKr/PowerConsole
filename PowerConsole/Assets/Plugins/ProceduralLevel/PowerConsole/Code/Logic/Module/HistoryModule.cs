using ProceduralLevel.Serialization;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class HistoryModule: AConsoleModule
	{
		public const int EXECUTION_HISTORY_LIMIT = 50;

		private List<string> m_ExecutionHistory = new List<string>(EXECUTION_HISTORY_LIMIT);

		public int Count
		{
			get { return m_ExecutionHistory.Count; }
		}

		public HistoryModule(ConsoleInstance console) : base(console)
		{
		}

		public override void BindEvents()
		{
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
			Write(Console.Persistence);
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
			Write(Console.Persistence);
		}
		#endregion

		#region Serialization
		private string HISTORY_FILE = "History";


		protected override string SavePath { get { return HISTORY_FILE; } }

		private const string KEY_HISTORY = "history";

		protected override void Serialize(AObject serializer)
		{
			base.Serialize(serializer);

			AArray entries = serializer.WriteArray(KEY_HISTORY);
			for(int x = 0; x < m_ExecutionHistory.Count; x++)
			{
				entries.Write(m_ExecutionHistory[x]);
			}
		}

		protected override void Deserialize(AObject serializer)
		{
			base.Deserialize(serializer);

			AArray entries = serializer.TryReadArray(KEY_HISTORY);
			if(entries != null)
			{
				for(int x = 0; x < entries.Count; x++)
				{
					m_ExecutionHistory.Add(entries.ReadString());
				}
			}
		}
		#endregion
	}
}
