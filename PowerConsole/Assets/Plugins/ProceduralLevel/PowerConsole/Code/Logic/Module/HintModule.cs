using ProceduralLevel.Common.Event;
using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class HintModule: AConsoleModule
	{
		public bool IteratingHints { get; private set; }

		public AConsoleCommand Command { get; private set; }
		public Query Query { get; private set; }
		public Argument Argument { get; private set; }
		public AHint Hint { get; private set; }

		private AHintIterator m_Iterator;
		public HintHit Current { get; private set; }
		
		private readonly HintManager m_HintsManager = new HintManager();

		public List<Exception> Issues = new List<Exception>();

		public readonly Event<HintHit> OnHintChanged = new Event<HintHit>();

		public HintModule(ConsoleInstance console) : base(console)
		{
		}

		public override void BindEvents()
		{
			Console.InputModule.OnInputChanged.AddListener(InputChangedHandler);
			Console.InputModule.OnCursorMoved.AddListener(CursorMovedHandler);
		}

		private void InputChangedHandler(string userInput)
		{
			UpdateHintStatus();
		}

		private void CursorMovedHandler(int cursor)
		{
			UpdateHintStatus();
		}

		private void UpdateHintStatus()
		{
			ParseQuery();
			FindHint();
			UpdateHintHit();
		}

		private void ParseQuery()
		{
			Clear();

			//parse query
			List<Query> queries;
			try
			{
				queries = Console.ParseQuery(Console.InputModule.UserInput);
			}
			catch(Exception e)
			{
				queries = new List<Query>();
				Issues.Add(e);
			}

			//find query at which cursor is currently
			for(int x = 0; x < queries.Count; x++)
			{
				Query = queries[x];
				Argument = Query.GetArgumentAt(Console.InputModule.Cursor);
				if(Argument != null)
				{
					break;
				}
			}

			//if there is a query, fnid the command then map and parse arguments
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
		}

		private void FindHint()
		{
			if(Argument != null)
			{
				if(Argument.Parameter != null)
				{
					Hint = Command.GetHintFor(m_HintsManager, Argument.Parameter.Index);
					m_Iterator = Hint.GetIterator(Argument.Value);
				}
				else if(Argument.IsCommandName)
				{
					Hint = (Query.IsOption? Console.OptionHint: Console.NameHint);
					m_Iterator = Hint.GetIterator(Argument.Value);
					Command = Console.FindCommand(m_Iterator.Current);
				}
				else
				{
					Clear();
				}
			}
			else
			{
				Clear();
				Hint = Console.NameHint;
				m_Iterator = Hint.GetIterator(string.Empty);
			}
		}

		private void UpdateHintHit()
		{
			HintHit old = Current;
			if(Query != null && Argument != null && m_Iterator != null)
			{
				if(Current == null || m_Iterator.Current != Current.Hint)
				{
					Current = new HintHit(Console.InputModule.UserInput, Argument, m_Iterator.Current);
					if(m_Iterator.Current.Length == 0)
					{
						IteratingHints = false;
					}
					OnHintChanged.Invoke(Current);
					//Console.InputState.SetCursor(Current.SufixOffset, true);
				}
			}
			else
			{
				Current = null;
				if(old != null)
				{
					OnHintChanged.Invoke(Current);
				}
			}
		}

		private void Clear()
		{
			Command = null;
			Query = null;
			Current = null;
			m_Iterator = null;
			IteratingHints = false;
			Issues.Clear();
		}

		#region Control
		public void NextHint()
		{
			if(IteratingHints && m_Iterator != null && !m_Iterator.MoveNext())
			{
				m_Iterator.Restart();
			}
			SetToCurrentHint();
		}

		public void PrevHint()
		{
			if(IteratingHints && m_Iterator != null && !m_Iterator.MovePrev())
			{
				m_Iterator.Restart();
			}
			SetToCurrentHint();
		}

		private void SetToCurrentHint()
		{
			if(m_Iterator != null && Argument != null && m_Iterator.Current != Argument.Value)
			{
				IteratingHints = true;
				int currentLength = m_Iterator.Current.Length;
				Console.InputModule.SetCursor(Argument.Offset+(currentLength > 0? currentLength: Argument.Value.Length), true);
				//SetCursor(hit.Prefix.Length+hit.HitPrefix.Length+hit.Value.Length+hit.HitSufix.Length);
			}
			UpdateHintHit();
		}

		public void CancelHint()
		{
			IteratingHints = false;
			Clear();
			UpdateHintStatus();
		}
		#endregion
	}
}
