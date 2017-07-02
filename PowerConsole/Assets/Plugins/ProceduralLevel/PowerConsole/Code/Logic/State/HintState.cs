using ProceduralLevel.Common.Event;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class HintState: AConsoleState
	{
		public bool IteratingHints { get; private set; }

		public AConsoleCommand Command { get; private set; }
		public Query Query { get; private set; }
		public Argument Argument { get; private set; }
		public AHint Hint { get; private set; }

		private AHintIterator m_Iterator;
		public HintHit Current { get; private set; }

		public readonly Event<HintHit> OnHintChanged = new Event<HintHit>();

		public HintState(ConsoleInstance console) : base(console)
		{
		}

		public override void BindEvents()
		{
			Console.InputState.OnInputChanged.AddListener(InputChangedHandler);
		}

		private void InputChangedHandler(InputState state)
		{
			IteratingHints = false;
			UpdateHint(state.Command, state.Query, state.Argument);
		}


		public void UpdateHint(AConsoleCommand command, Query query, Argument argument)
		{
			//find a way to limit call on this
			Hint = null;
			Command = command;
			Query = query;
			Argument = argument;
			if(Argument != null)
			{
				if(Argument.Parameter != null)
				{
					Hint = command.GetHintFor(Console.Hints, Argument.Parameter.Index);
					m_Iterator = Hint.GetIterator(argument.Value);
				}
				else if(argument.IsCommandName)
				{
					Hint = (query.IsOption? Console.OptionHint: Console.NameHint);
					m_Iterator = Hint.GetIterator(argument.Value);
					Command = Console.FindCommand(m_Iterator.Current);
				}
				else
				{
					Clear();
				}
			}
			else
			{
				Command = null;
				Query = null;
				Hint = Console.NameHint;
				m_Iterator = Hint.GetIterator(string.Empty);
			}
			RefreshCurrent();
		}

		private void RefreshCurrent()
		{
			HintHit old = Current;
			if(Query != null && Argument != null && m_Iterator != null)
			{
				Current = new HintHit(Console.InputState.UserInput, Argument, m_Iterator.Current);
				if(Argument.IsCommandName)
				{
					Command = Console.FindCommand(m_Iterator.Current);
				}
				if(m_Iterator.Current.Length == 0)
				{ 
					IteratingHints = false;
				}
			}
			else
			{
				Current = null;
				IteratingHints = false;
			}
			OnHintChanged.Invoke(Current);
		}

		private void Clear()
		{
			Command = null;
			Query = null;
			Current = null;
			m_Iterator = null;
		}

		#region Control
		public void NextHint()
		{
			if(m_Iterator != null && !m_Iterator.MoveNext())
			{
				m_Iterator.Restart();
			}
			SetToCurrentHint();
		}

		public void PrevHint()
		{
			if(m_Iterator != null && !m_Iterator.MovePrev())
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
				Console.InputState.SetCursor(Argument.Offset+m_Iterator.Current.Length);
			}
			RefreshCurrent();
		}

		public void CancelHint()
		{
			IteratingHints = false;
			Clear();
			RefreshCurrent();
			InputChangedHandler(Console.InputState);
		}
		#endregion
	}
}
