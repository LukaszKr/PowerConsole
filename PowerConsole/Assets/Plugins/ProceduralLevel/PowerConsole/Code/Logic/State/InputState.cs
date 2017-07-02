using ProceduralLevel.Common.Event;
using System;
using System.Collections.Generic;

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

		public List<Exception> Issues = new List<Exception>();

		public readonly Event<InputState> OnInputChanged = new Event<InputState>();
		public readonly Event<int> OnCursorMoved = new Event<int>();

		public AConsoleCommand Command { get; private set; }
		public Query Query { get; private set; }
		public Argument Argument { get; private set; }

		private int m_HistoryIndex;

		public InputState(ConsoleInstance console)
			: base(console)
		{
			UserInput = null;
			Cursor = -1;
			m_HistoryIndex = -1;
		}

		public override void BindEvents()
		{
			Console.HintState.OnHintChanged.AddListener(HintChangedListener);
		}

		public void SetInput(string userInput, int cursor)
		{
			if(UserInput == userInput && Cursor == cursor)
			{
				return;
			}

			SetCursor(cursor);
			UserInput = userInput;

			Command = null;
			Query = null;
			Argument = null;
			Issues.Clear();

			List<Query> queries;
			try
			{
				queries = Console.ParseQuery(UserInput);
			}
			catch(Exception e)
			{
				queries = new List<Query>();
				Issues.Add(e);
			}

			for(int x = 0; x < queries.Count; x++)
			{
				Query = queries[x];
				Argument = Query.GetArgumentAt(cursor);
				if(Argument != null)
				{
					break;
				}
			}

			if(Query != null)
			{
				Command = Console.FindCommand(Query.Name.Value);
				if(Command != null)
				{
					try
					{
						Command.Method.MapArguments(Query);
					}
					catch(Exception e)
					{
						Issues.Add(e);
					}
					try
					{
						Console.ParseValues(Query);
					}
					catch(Exception e)
					{
						Issues.Add(e);
					}
				}
			}

			OnInputChanged.Invoke(this);
		}

		public void SetCursor(int cursor, bool force = false)
		{ 
			if(force || Cursor != cursor)
			{
				Cursor = cursor;
				OnCursorMoved.Invoke(Cursor);
			}
		}

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

		private void HintChangedListener(HintHit hit)
		{
			if(hit != null && Console.HintState.IteratingHints)
			{
				SetCursor(Argument.Offset+hit.HitPrefix.Length+hit.Value.Length+hit.HitSufix.Length);
			}
		}

		#region Control
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
			UserInput = "";
			OnInputChanged.Invoke(this);
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
