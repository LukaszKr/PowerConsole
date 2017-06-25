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
		public string CurrentHint { get { return m_Iterator.Current; } }

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
					Hint = Console.NameHint;
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
				Hint = Console.NameHint;
				m_Iterator = Hint.GetIterator(string.Empty);
				Clear();
			}
			RefreshCurrent();
		}

		private void RefreshCurrent()
		{
			if(Query != null && Argument != null && m_Iterator != null)
			{
				Current = new HintHit(Query, Argument, m_Iterator.Current);
			}
			else
			{
				Current = null;
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
			IteratingHints = true;
			RefreshCurrent();
		}

		public void PrevHint()
		{
			if(m_Iterator != null && !m_Iterator.MovePrev())
			{
				m_Iterator.Restart();
			}
			IteratingHints = true;
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
