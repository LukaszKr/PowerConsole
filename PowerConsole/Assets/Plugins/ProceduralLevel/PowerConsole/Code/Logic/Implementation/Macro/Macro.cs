using ProceduralLevel.Serialization;
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

		public void Serialize(AObject serializer)
		{
			serializer.Write(KEY_NAME, Name);
			AArray queries = serializer.WriteArray(KEY_QUERIES);
			for(int x = 0; x < Queries.Count; x++)
			{
				queries.Write(Queries[x]);
			}
		}

		public void Deserialize(AObject serializer)
		{
			Name = serializer.ReadString(KEY_NAME);
			Queries.Clear();
			AArray queries = serializer.ReadArray(KEY_QUERIES);
			for(int x = 0; x < queries.Count; x++)
			{
				Queries.Add(queries.ReadString());
			}
		}
		#endregion
	}
}
