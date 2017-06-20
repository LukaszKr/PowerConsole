using ProceduralLevel.PowerConsole.Logic;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleView: MonoBehaviour
	{
		public ConsoleInputPanel Input;
		public ConsoleMessagesPanel Messages;
		
		private ConsoleInstance m_Console;

		public void Awake()
		{
			m_Console = new ConsoleInstance(new DefaultLocalization());
			Input.Initialize(m_Console);
			Messages.Initialize(m_Console);

			m_Console.OnMessage.Invoke(new Message(EMessageType.Info, "PowerConsole by Procedural Level"));
			m_Console.OnMessage.Invoke(new Message(EMessageType.Info, "Very long message has to go here, to test if multiline is properly supported." +
				"Event thou this might be a tricky task to do, it has to be supported. Lorem ipsum could also be here, but come on, let's be creative!"));
			m_Console.OnMessage.Invoke(new Message(EMessageType.Info, "Hello World!"));
			for(int x = 0; x < 30; x++)
			{
				m_Console.OnMessage.Invoke(new Message(EMessageType.Warning, x.ToString()));
			}
		}
	}
}
