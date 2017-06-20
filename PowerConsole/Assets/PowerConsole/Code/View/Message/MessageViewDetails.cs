using ProceduralLevel.PowerConsole.Logic;

namespace ProceduralLevel.PowerConsole.View
{
	public class MessageViewDetails
	{
		public readonly Message Message;
		public float Height;

		public MessageViewDetails(Message message, float height)
		{
			Message = message;
			Height = height;
		}
	}
}
