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
				TextAsset localization = Target.LocalizationCSV;
				string path = AssetDatabase.GetAssetPath(localization);
				string[] lines =  File.ReadAllLines(path);
				CSVParser parser = new CSVParser();
				for(int x = 0; x < lines.Length; x++)
				{
					parser.Parse(lines[x]);
				}
				CSV csv = parser.Flush();
				csv.TryAddHeaders("key", "en-us");
				
				string[] keys = Enum.GetNames(typeof(ELocalizationKey));
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

				if(Provider.onlineState != OnlineState.Offline)
				{
					Provider.Checkout(localization, CheckoutMode.Asset).Wait();
				}
				File.WriteAllText(path, csv.ToString());
			}
		}
	}
}
