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

		public AConsoleCommand Command { get; private set; }
		public Query Query { get; private set; }
		public Argument Argument { get; private set; }

		public InputState(ConsoleInstance console)
			: base(console)
		{
		}

		public void SetInput(string userInput, int cursor)
		{
			if(UserInput == userInput && Cursor == cursor)
			{
				return;
			}

			UserInput = userInput;
			Cursor = cursor;

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
