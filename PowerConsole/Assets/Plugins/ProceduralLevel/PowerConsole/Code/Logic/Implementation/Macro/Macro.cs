using ProceduralLevel.Common.Serialization;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class Macro: IObjectSerializable
	{
		public string Name;
		public List<string> Queries = new List<string>();

		#region Serialization
		private string KEY_NAME = "name";
		private string KEY_QUERIES = "queries";
		
		public Macro() { }

		public Macro(string name)
		{
			Name = name;
		}

		public void Serialize(IObjectSerializer serializer)
		{
			serializer.Write(KEY_NAME, Name);
			IArraySerializer queries = serializer.WriteArray(KEY_QUERIES);
			for(int x = 0; x < Queries.Count; x++)
			{
				queries.Write(Queries[x]);
			}
		}

		public void Deserialize(IObjectSerializer serializer)
		{
			Name = serializer.ReadString(KEY_NAME);
			Queries.Clear();
			IArraySerializer queries = serializer.ReadArray(KEY_QUERIES);
			for(int x = 0; x < queries.Count; x++)
			{
				Queries.Add(queries.ReadString());
			}
		}
		#endregion
	}
}
