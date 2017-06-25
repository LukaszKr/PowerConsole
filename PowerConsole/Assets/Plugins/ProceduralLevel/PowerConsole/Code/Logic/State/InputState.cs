using ProceduralLevel.Common.Event;
using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class InputState: AConsoleState
	{
		public string UserInput {  get; private set; }
		public int Cursor { get; private set; }

		public List<Exception> Issues = new List<Exception>();

		public readonly Event<InputState> OnInputChanged = new Event<InputState>();
		public readonly Event<int> OnCursorMoved = new Event<int>();

		public AConsoleCommand Command { get; private set; }
		public Query Query { get; private set; }
		public Argument Argument { get; private set; }

		public InputState(ConsoleInstance console)
			: base(console)
		{
			UserInput = "";
			Cursor = 0;
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

			UserInput = userInput;
			SetCursor(Cursor);

			Command = null;
			Query = null;
			Argument = null;

			List<Query> queries = Console.ParseQuery(UserInput);

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

		public void SetCursor(int cursor)
		{ 
			if(Cursor != cursor)
			{
				Cursor = cursor;
				OnCursorMoved.Invoke(Cursor);
			}
		}

		private void HintChangedListener(HintHit hit)
		{
			if(hit != null && Console.HintState.IteratingHints)
			{
				SetCursor(Argument.Offset+Console.HintState.CurrentHint.Length);
			}
			else if(Argument != null)
			{
				SetCursor(Argument.Offset+Argument.Value.Length);
			}
		}

		#region Control
		public void Execute()
		{
			Console.Execute(UserInput);
			UserInput = "";
			OnInputChanged.Invoke(this);
		}
		#endregion
	}
}
