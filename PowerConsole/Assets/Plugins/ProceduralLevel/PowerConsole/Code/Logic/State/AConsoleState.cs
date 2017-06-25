using ProceduralLevel.Common.Parsing;
using ProceduralLevel.Common.Serialization;

namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class AConsoleState
	{
		public readonly ConsoleInstance Console;

		public AConsoleState(ConsoleInstance console)
		{
			Console = console;
		}

		public abstract void BindEvents();

		#region Serialization
		private const string FILE_EXT = ".json";

		protected virtual string SavePath { get { return string.Empty; } }

		private bool IsPersistent { get { return !string.IsNullOrEmpty(SavePath); } }

		public virtual void Write(IPersistence persistence) 
		{ 
			if(IsPersistent)
			{
				JsonObjectSerializer json = new JsonObjectSerializer();
				Serialize(json);
				persistence.WriteFile(SavePath+FILE_EXT, json.ToString());
			}
		}
		
		public virtual void Read(IPersistence persistence) 
		{
			if(IsPersistent)
			{
				string data = persistence.ReadFile(SavePath+FILE_EXT);
				if(!string.IsNullOrEmpty(data))
				{
					JsonObjectSerializer serializer = null;
					try
					{
						JsonParser parser = new JsonParser();
						parser.Parse(data);
						serializer = new JsonObjectSerializer(parser.Flush());
					}
					finally
					{
						if(serializer != null)
						{
							Deserialize(serializer);
						}
					}
				}
			}
		}

		protected virtual void Deserialize(IObjectSerializer serializer) { }

		protected virtual void Serialize(IObjectSerializer serializer) { }
		#endregion
	}
}
