using ProceduralLevel.PowerConsole.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleInput
	{
		private KeyCode[] m_Execute = new KeyCode[] { KeyCode.Return };
		private KeyCode[] m_NextHint = new KeyCode[] { KeyCode.Tab };
		private KeyCode[] m_NextHistory = new KeyCode[] { KeyCode.DownArrow };
		private KeyCode[] m_PrevHistory = new KeyCode[] { KeyCode.UpArrow };

		private List<InputAction> m_Actions = new List<InputAction>();

		private ConsoleInstance m_Console;

		public ConsoleInput(ConsoleInstance console)
		{
			m_Console = console;

			m_Actions.Add(new InputAction(m_Execute, console.InputState.Execute));
			m_Actions.Add(new InputAction(m_NextHint, console.HintState.NextHint));
			m_Actions.Add(new InputAction(m_NextHistory, console.InputState.NextHistory));
			m_Actions.Add(new InputAction(m_PrevHistory, console.InputState.PrevHistory));
		}

		public void Update()
		{
			for(int x = 0; x < m_Actions.Count; x++)
			{ 
				InputAction action = m_Actions[x];
				TryInvoke(action.Keys, action.Callback);
			}
		}

		#region Callback
		private void Execute()
		{
			m_Console.InputState.Execute();
		}

		private void NextHint()
		{
			m_Console.HintState.NextHint();
		}
		#endregion

		private void TryInvoke(KeyCode[] keys, Action action)
		{
			if(TryConsumeKeys(keys))
			{
				action();
			}
		}

		private bool TryConsumeKeys(KeyCode[] keyCodes)
		{
			for(int x = 0; x < keyCodes.Length; x++)
			{
				if(TryConsumeKey(keyCodes[x]))
				{
					return true;
				}
			}
			return false;
		}

		private bool TryConsumeKey(KeyCode keyCode)
		{
			Event evnt = Event.current;
			if(evnt != null && evnt.type == EventType.KeyUp && evnt.keyCode == keyCode)
			{
				evnt.Use();
				return true;
			}
			return false;
		}
	}
}
