using ProceduralLevel.Common.Event;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class HintState: AConsoleState
	{
		public bool IteratingHints { get; private set; }

		public AConsoleCommand Command { get; private set; }
		public Query Query { get; private set; }
		public Argument Argument { get; private set; }
		public AHint Current { get; private set; }

		private AHintIterator m_Iterator;
		private HintHit m_CurrentHint;

		public readonly Event<HintHit> OnHintChanged = new Event<HintHit>();

		public HintState(ConsoleInstance console) : base(console)
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
			if(Argument != argument)
			{
				Current = null;
				Command = command;
				Query = query;
				Argument = argument;
				if(Argument != null)
				{
					if(Argument.Parameter != null)
					{
						Current = command.GetHintFor(Console.Hints, Argument.Parameter.Index);
						m_Iterator = Current.GetIterator(argument.Value);
					}
					else if(argument.IsCommandName)
					{
						Current = Console.NameHint;
						m_Iterator = Current.GetIterator(argument.Value);
						Command = Console.FindCommand(m_Iterator.Current);
					}
					else
					{
						Clear();
					}
				}
				else
				{
					Current = Console.NameHint;
					m_Iterator = Current.GetIterator(string.Empty);
					Clear();
				}
				RefreshCurrent();
			}
		}

		private void RefreshCurrent()
		{
			if(Query != null && Argument != null && m_Iterator != null)
			{
				m_CurrentHint = new HintHit(Query, Argument, m_Iterator.Current);
			}
			else
			{
				m_CurrentHint = null;
			}
			OnHintChanged.Invoke(m_CurrentHint);
		}

		private void Clear()
		{
			Command = null;
			Query = null;
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
		#endregion
	}
}
