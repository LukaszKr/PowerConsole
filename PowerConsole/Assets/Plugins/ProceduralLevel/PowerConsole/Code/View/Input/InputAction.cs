using System;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class InputAction
	{
		public readonly KeyCode[] Keys;
		public readonly Action Callback;

		public InputAction(KeyCode[] keys, Action callback)
		{
			Keys = keys;
			Callback = callback;
		}
	}
}
