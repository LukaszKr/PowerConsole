namespace ProceduralLevel.PowerConsole.Logic
{
	public class Message
	{
		public readonly EMessageType Type = EMessageType.Normal;
		public readonly string Value;

		public Message(EMessageType type, string value)
		{
			Type = type;
			Value = value;
		}

		public override string ToString()
		{
			return string.Format("[{0}] {1}", Type.ToString(), Value.ToString());
		}
	}
}
