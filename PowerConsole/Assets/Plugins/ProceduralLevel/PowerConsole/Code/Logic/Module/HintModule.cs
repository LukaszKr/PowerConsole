using ProceduralLevel.Common.Event;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class HintModule: AConsoleModule
	{
		public bool IteratingHints { get; private set; }

		private HintProvider m_HintProvider;
		public AHintIterator Iterator { get; private set; }
		public HintHit Current { get; private set; }

		public List<ConsoleException> Issues = new List<ConsoleException>();

		public readonly Event<HintHit> OnHintChanged = new Event<HintHit>();

		public HintModule(ConsoleInstance console) : base(console)
		{
			m_HintProvider = new HintProvider(console);
		}

		public override void BindEvents()
		{
			Console.InputModule.OnInputChanged.AddListener(InputChangedHandler);
		}

		private void InputChangedHandler(string userInput)
		{
			UpdateHint();
		}

		public void UpdateHint()
		{
			Issues.Clear();
			Iterator = m_HintProvider.GetHintIterator(Console.InputModule.UserInput, Console.InputModule.Cursor, Issues);
			IteratingHints = false;
			SetToCurrentHint();
		}

		#region Control
		public void NextHint()
		{
			if(IteratingHints && Iterator != null && !Iterator.MoveNext())
			{
				Iterator.Restart();
			}
			IteratingHints = true;
			SetToCurrentHint();
		}

		public void PrevHint()
		{
			if(IteratingHints && Iterator != null && !Iterator.MovePrev())
			{
				Iterator.Restart();
			}
			IteratingHints = true;
			SetToCurrentHint();
		}

		private void SetToCurrentHint()
		{
			if(Iterator != null)
			{
				Current = new HintHit(Console.InputModule.UserInput, Iterator, Iterator.Current);
				OnHintChanged.Invoke(Current);
			}
			else
			{
				bool hadValue = Current != null;
				Current = null;
				if(hadValue)
				{
					OnHintChanged.Invoke(Current);
				}
			}
		}

		public void CancelHint()
		{
			Console.InputModule.SetCursor(Current.SufixOffset-Current.HitSufix.Length);
			UpdateHint();
		}
		#endregion
	}
}
