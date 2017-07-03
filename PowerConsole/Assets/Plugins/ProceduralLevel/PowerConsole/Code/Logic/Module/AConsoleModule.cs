using ProceduralLevel.Serialization;
using ProceduralLevel.Serialization.Json;

namespace ProceduralLevel.PowerConsole.Logic
{
	public abstract class AConsoleModule
	{
		public readonly ConsoleInstance Console;

		public AConsoleModule(ConsoleInstance console)
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
				JsonObject json = new JsonObject();
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
					JsonObject serializer = null;
					try
					{
						JsonParser parser = new JsonParser();
						parser.Parse(data);
						serializer = parser.Flush();
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

		protected virtual void Deserialize(AObject serializer) { }

		protected virtual void Serialize(AObject serializer) { }
		#endregion
	}
}
