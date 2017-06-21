using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.View
{
	public class MessageView
	{
		public readonly Message Message;
		public float Height;

		public MessageView(Message message, float height)
		{
			Message = message;
			Height = height;
		}
	}
}
