using ProceduralLevel.PowerConsole.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleInput
	{
		private KeyCode[] m_Toggle = new KeyCode[] { KeyCode.BackQuote };
		private KeyCode[] m_Execute = new KeyCode[] { KeyCode.Return };
		private KeyCode[] m_NextHint = new KeyCode[] { KeyCode.Tab };
		private KeyCode[] m_NextHistory = new KeyCode[] { KeyCode.UpArrow };
		private KeyCode[] m_PrevHistory = new KeyCode[] { KeyCode.DownArrow };

		private List<InputAction> m_Actions = new List<InputAction>();

		private ConsoleView m_View;
		private ConsoleInstance m_Console;

		public ConsoleInput(ConsoleView view)
		{
			m_View = view;
			m_Console = view.Console;

			m_Actions.Add(new InputAction(m_Execute, m_Console.InputModule.Execute));
			m_Actions.Add(new InputAction(m_NextHint, m_Console.HintModule.NextHint));
			m_Actions.Add(new InputAction(m_PrevHistory, m_Console.InputModule.PrevHistory));
			m_Actions.Add(new InputAction(m_NextHistory, m_Console.InputModule.NextHistory));
			m_Actions.Add(new InputAction(m_Toggle, m_View.ToggleActive));
		}

		public void Update()
		{
			for(int x = 0; x < m_Actions.Count; x++)
			{ 
				InputAction action = m_Actions[x];
				TryInvoke(action.Keys, action.Callback);
			}
		}

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
