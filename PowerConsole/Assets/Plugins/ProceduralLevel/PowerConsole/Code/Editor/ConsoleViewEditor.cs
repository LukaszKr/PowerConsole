using ProceduralLevel.ExtendedEditor;
using ProceduralLevel.PowerConsole.Logic;
using ProceduralLevel.Serialization.CSV;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	[CustomEditor(typeof(ConsoleView))]
	public class ConsoleViewEditor: AExtendedEditor<ConsoleView>
	{
		protected override void Initialize()
		{
			DrawDefault = true;
		}

		protected override void Draw()
		{
			if(GUILayout.Button("Update Localization"))
			{
				TextAsset asset = Target.LocalizationCSV;
				string path = AssetDatabase.GetAssetPath(asset);
				CSVObject csv = LoadCSV(path);
				AddMissingKeys(csv);
				SaveCSV(asset, path, csv);
			}
		}

		private void AddMissingKeys(CSVObject csv)
		{
			string[] keys = Enum.GetNames(typeof(ELocKey));
			for(int x = 0; x < keys.Length; x++)
			{
				if(csv.Find(0, keys[x]) == null)
				{
					CSVEntry entry = new CSVEntry(csv.Width);
					for(int entryColumn = 0; entryColumn < entry.Size; entryColumn++)
					{
						entry[entryColumn] = keys[entryColumn];
					}
					csv.Add(entry);
				}
			}
		}

		private CSVObject LoadCSV(string path)
		{
			string[] lines = File.ReadAllLines(path);
			CSVParser parser = new CSVParser();
			for(int x = 0; x < lines.Length; x++)
			{
				parser.Parse(lines[x]);
			}

			CSVObject csv = parser.Flush();

			
			csv.AddColumns("key", "en-us");

			string[] keyNames = Enum.GetNames(typeof(ELocKey));
			HashSet<string> knownNames = new HashSet<string>();


			for(int x = 0; x < keyNames.Length; x++)
			{
				string key = keyNames[x];
				if(!knownNames.Contains(key))
				{
					CSVEntry row = new CSVEntry(csv.Width);
					for(int iRow = 0; iRow < row.Size; iRow++)
					{
						row[iRow] = key;
					}
					csv.Add(row);
				}
			}

			return csv;
		}

		private void SaveCSV(TextAsset asset, string path, CSVObject csv)
		{
			if(Provider.onlineState != OnlineState.Offline)
			{
				Provider.Checkout(asset, CheckoutMode.Asset).Wait();
			}
			File.WriteAllText(path, csv.ToString());
		}
	}
}
