using ProceduralLevel.PowerConsole.Logic;
using System.IO;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class UnityPersistence: IPersistence
	{
		private const string MAIN_FOLDER = "PowerConsole";

		public string ReadFile(string filePath)
		{
			string path = GetPath(filePath);
			FileInfo file = new FileInfo(path);
			if(file.Exists)
			{
				return File.ReadAllText(path);
			}
			else
			{
				return string.Empty;
			}
		}

		public void WriteFile(string filePath, string data)
		{
			string path = GetPath(filePath);
			File.WriteAllText(path, data);
		}

		private string GetPath(string file)
		{
			string path = Path.Combine(Path.Combine(Application.persistentDataPath, MAIN_FOLDER), file);
			DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(path));
			if(!directory.Exists)
			{
				directory.Create();
			}
			return path;
		}
	}
}
