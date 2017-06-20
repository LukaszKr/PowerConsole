using ProceduralLevel.PowerConsole.Logic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public abstract class AConsoleMessageView: MonoBehaviour
	{
		protected Message m_Message;

		public abstract float Height { get; }

		public void SetMessage(Message message)
		{
			m_Message = message;
			RefreshView();
		}

		protected abstract void RefreshView();
	}
}
