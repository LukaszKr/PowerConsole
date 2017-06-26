using ProceduralLevel.Common.Parsing;
using ProceduralLevel.ExtendedEditor;
using ProceduralLevel.PowerConsole.Logic;
using System;
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
				CSV csv = LoadCSV(path);
				AddMissingKeys(csv);
				SaveCSV(asset, path, csv);
			}
		}

		private void AddMissingKeys(CSV csv)
		{
			string[] keys = Enum.GetNames(typeof(ELocKey));
			for(int x = 0; x < keys.Length; x++)
			{
				if(csv.FindRow(0, keys[x]) == null)
				{
					CSVRow entry = new CSVRow(csv.Header.Length);
					for(int entryColumn = 0; entryColumn < entry.Length; entryColumn++)
					{
						entry[entryColumn] = keys[entryColumn];
					}
					csv.Add(entry);
				}
			}
		}

		private CSV LoadCSV(string path)
		{
			string[] lines = File.ReadAllLines(path);
			CSVParser parser = new CSVParser();
			for(int x = 0; x < lines.Length; x++)
			{
				parser.Parse(lines[x]);
			}

			CSV csv = parser.Flush();
			csv.TryAddHeaders("key", "en-us");
			
			return csv;
		}

		private void SaveCSV(TextAsset asset, string path, CSV csv)
		{
			if(Provider.onlineState != OnlineState.Offline)
			{
				Provider.Checkout(asset, CheckoutMode.Asset).Wait();
			}
			File.WriteAllText(path, csv.ToString());
		}
	}
}
