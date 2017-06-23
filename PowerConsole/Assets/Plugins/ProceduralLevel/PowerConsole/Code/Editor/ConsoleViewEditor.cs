using ProceduralLevel.Common.Parsing;
using ProceduralLevel.ExtendedEditor;
using System.IO;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.View
{
	public class ConsoleViewEditor: AExtendedEditor<ConsoleView>
	{
		protected override void Initialize()
		{
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
				

				Provider.Checkout(localization, CheckoutMode.Asset).Wait();
				File.WriteAllText(path, csv.ToString());
			}
		}
	}
}
