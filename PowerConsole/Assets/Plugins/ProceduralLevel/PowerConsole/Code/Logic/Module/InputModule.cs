using ProceduralLevel.Common.Event;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class InputModule: AConsoleModule
	{
		public string CurrentInput 
		{ 
			get 
			{
				if(Console.HintModule.IteratingHints && Console.HintModule.Current != null)
				{
					return Console.HintModule.Current.Merged;
				}
				else
				{
					return Console.InputModule.UserInput;
				}
			}
		}
		public string UserInput { get; private set; }
		public int Cursor { get; private set; }


		public readonly Event<string> OnInputChanged = new Event<string>();
		public readonly Event<int> OnCursorMoved = new Event<int>();

		private int m_HistoryIndex;

		public InputModule(ConsoleInstance console)
			: base(console)
		{
			UserInput = "";
			Cursor = -1;
			m_HistoryIndex = -1;
		}

		public override void BindEvents()
		{
			Console.HintModule.OnHintChanged.AddListener(HintChangedHandler);
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
			if(Console.HintModule.IteratingHints)
			{
				Console.Execute(Console.HintModule.Current.Merged);
			}
			else
			{
				Console.Execute(UserInput);
			}
			SetInput("", 0);
		}

		private void HintChangedHandler(HintHit current)
		{
			if(current != null && Console.HintModule.IteratingHints)
			{
				SetCursor(current.SufixOffset);
			}
		}

		#region History
		private void IterateHistory(int indexChange)
		{
			int executionCount = Console.HistoryModule.Count;
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
				string input = Console.HistoryModule.Get(m_HistoryIndex);
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
