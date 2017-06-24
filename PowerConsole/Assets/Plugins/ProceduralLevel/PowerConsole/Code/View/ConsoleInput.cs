using System;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleInput
	{
		private KeyCode[] m_Execute = new KeyCode[] { KeyCode.Return };
		private KeyCode[] m_NextHint = new KeyCode[] { KeyCode.Tab };

		public void Update(ConsoleView view)
		{
			TryInvoke(m_Execute, view.Input.Execute);
			TryInvoke(m_NextHint, view.Hints.NextHint);
		}

		public bool ExecuteCommand()
		{
			return TryConsumeKeys(m_Execute);
		}

		public bool NextHint()
		{
			return TryConsumeKeys(m_NextHint);
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
