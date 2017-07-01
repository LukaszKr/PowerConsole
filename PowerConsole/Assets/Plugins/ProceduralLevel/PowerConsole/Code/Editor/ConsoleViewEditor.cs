//using ProceduralLevel.ExtendedEditor;
//using ProceduralLevel.PowerConsole.Logic;
//using ProceduralLevel.Serialization.CSV;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEditor.VersionControl;
//using UnityEngine;

//namespace ProceduralLevel.PowerConsole.View
//{
//	[CustomEditor(typeof(ConsoleView))]
//	public class ConsoleViewEditor: AExtendedEditor<ConsoleView>
//	{
//		protected override void Initialize()
//		{
//			DrawDefault = true;
//		}

//		protected override void Draw()
//		{
//			if(GUILayout.Button("Update Localization"))
//			{
//				TextAsset asset = Target.LocalizationCSV;
//				string path = AssetDatabase.GetAssetPath(asset);
//				CSVObject csv = LoadCSV(path);
//				AddMissingKeys(csv);
//				SaveCSV(asset, path, csv);
//			}
//		}

//		private void AddMissingKeys(CSVObject csv)
//		{
//			string[] keys = Enum.GetNames(typeof(ELocKey));
//			for(int x = 0; x < keys.Length; x++)
//			{
//				if(csv.FindRow(0, keys[x]) == null)
//				{
//					CSVEntry entry = new CSVEntry(csv.Header.Length);
//					for(int entryColumn = 0; entryColumn < entry.Length; entryColumn++)
//					{
//						entry[entryColumn] = keys[entryColumn];
//					}
//					csv.Add(entry);
//				}
//			}
//		}

//		private CSV LoadCSV(string path)
//		{
//			string[] lines = File.ReadAllLines(path);
//			CSVParser parser = new CSVParser();
//			for(int x = 0; x < lines.Length; x++)
//			{
//				parser.Parse(lines[x]);
//			}

//			CSV csv = parser.Flush();
//			csv.TryAddHeaders("key", "en-us");

//			string[] keyNames = Enum.GetNames(typeof(ELocKey));
//			HashSet<string> knownNames = new HashSet<string>();


//			for(int x = 0; x < keyNames.Length; x++)
//			{
//				string key = keyNames[x];
//				if(!knownNames.Contains(key))
//				{
//					CSVRow row = new CSVRow(csv.Header.Length);
//					for(int iRow = 0; iRow < row.Length; iRow++)
//					{
//						row[iRow] = key;
//					}
//					csv.Add(row);
//				}
//			}

//			return csv;
//		}

//		private void SaveCSV(TextAsset asset, string path, CSV csv)
//		{
//			if(Provider.onlineState != OnlineState.Offline)
//			{
//				Provider.Checkout(asset, CheckoutMode.Asset).Wait();
//			}
//			File.WriteAllText(path, csv.ToString());
//		}
//	}
//}
