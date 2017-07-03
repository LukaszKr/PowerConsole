using ProceduralLevel.Common.Event;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class InputState: AConsoleState
	{
		public string CurrentInput 
		{ 
			get 
			{
				if(Console.HintState.IteratingHints && Console.HintState.Current != null)
				{
					return Console.HintState.Current.Merged;
				}
				else
				{
					return Console.InputState.UserInput;
				}
			}
		}
		public string UserInput { get; private set; }
		public int Cursor { get; private set; }


		public readonly Event<string> OnInputChanged = new Event<string>();
		public readonly Event<int> OnCursorMoved = new Event<int>();

		private int m_HistoryIndex;

		public InputState(ConsoleInstance console)
			: base(console)
		{
			UserInput = "";
			Cursor = -1;
			m_HistoryIndex = -1;
		}

		public override void BindEvents()
		{
		}

		public void SetInput(string userInput, int cursor)
		{
			if(UserInput == userInput && Cursor == cursor)
			{
				return;
			}

			SetCursor(cursor);
			UserInput = userInput;

			OnInputChanged.Invoke(UserInput);
		}

		public void SetCursor(int cursor, bool force = false)
		{ 
			if(force || Cursor != cursor)
			{
				Cursor = cursor;
				OnCursorMoved.Invoke(Cursor);
			}
		}

		public void Execute()
		{
			m_HistoryIndex = -1;
			if(UserInput.Trim().Length == 0)
			{
				return;
			}
			if(Console.HintState.IteratingHints)
			{
				Console.Execute(Console.HintState.Current.Merged);
			}
			else
			{
				Console.Execute(UserInput);
			}
			SetInput("", 0);
		}

		#region History
		private void IterateHistory(int indexChange)
		{
			int executionCount = Console.HistoryState.Count;
			m_HistoryIndex += indexChange;
			if(m_HistoryIndex < -1)
			{
				m_HistoryIndex = executionCount-1;
			}
			else if(m_HistoryIndex > executionCount)
			{
				m_HistoryIndex = 0;
			}
			if(m_HistoryIndex >= 0 && m_HistoryIndex < executionCount)
			{
				string input = Console.HistoryState.Get(m_HistoryIndex);
				SetInput(input, input.Length);
			}
			else
			{
				SetInput("", 0);
			}
		}

		public void NextHistory()
		{
			IterateHistory(-1);
		}

		public void PrevHistory()
		{
			IterateHistory(1);
		}
		#endregion
	}
}
