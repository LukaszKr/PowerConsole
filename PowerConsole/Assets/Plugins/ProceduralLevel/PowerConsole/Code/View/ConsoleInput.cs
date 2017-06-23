using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleInput
	{
		private KeyCode[] m_ExecuteKeys = new KeyCode[] { KeyCode.Return };

		public bool ExecuteCommand()
		{
			return TryConsumeKeys(m_ExecuteKeys);
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
