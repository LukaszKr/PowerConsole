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

		public override string ToString()
		{
			return string.Format("[{0}] {1}", Result.ToString(), Value.ToString());
		}
	}
}
