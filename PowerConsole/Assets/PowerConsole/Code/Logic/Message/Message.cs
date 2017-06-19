namespace ProceduralLevel.PowerConsole.Logic
{
	public class Message
	{
		public readonly EMessageType Result;
		public readonly string Value;

		public Message(EMessageType type, string value)
		{
			Result = type;
			Value = value;
		}
	}
}
