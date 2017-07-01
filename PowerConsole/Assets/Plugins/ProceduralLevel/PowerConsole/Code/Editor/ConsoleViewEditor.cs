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
		private CSVObject m_CSV;

		private int m_EditedHeader = 0;
		private string[] m_Headers;

		protected override void Initialize()
		{
			DrawDefault = true;
			m_CSV = LoadCSV();
		}

		private CSVObject LoadCSV()
		{
			m_Headers = null;
			TextAsset asset = Target.LocalizationCSV;
			if(asset != null)
			{
				string path = AssetDatabase.GetAssetPath(asset);
				CSVObject csv = LoadCSV(path);
				if(csv != null)
				{
					m_Headers = new string[csv.Header.Size-1];
					for(int x = 1; x < csv.Header.Size; x++)
					{
						m_Headers[x-1] = csv.Header[x];
					}
				}
				return csv;
			}
			return null;
		}

		protected override void Draw()
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Reload"))
			{
				m_CSV = LoadCSV();
				AddMissingKeys(m_CSV);
				SaveCSV();
			}
			if(GUILayout.Button("Save"))
			{
				SaveCSV();
			}
			EditorGUILayout.EndHorizontal();

			if(m_CSV != null)
			{
				DrawLocalizationEntries();
			}
		}

		private void DrawLocalizationEntries()
		{
			m_EditedHeader = EditorGUILayout.Popup(m_EditedHeader, m_Headers);
			int column = m_EditedHeader+1;
			EditorGUILayout.BeginVertical();

			for(int x = 0; x < m_CSV.Count; x++)
			{
				EditorGUILayout.BeginHorizontal();
				CSVEntry entry = m_CSV[x];
				string oldValue = entry[column];
				entry[column] = EditorGUILayout.DelayedTextField(entry[0], entry[column]);
				if(entry[column] != oldValue)
				{
					SaveCSV();
				}
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndVertical();
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
				parser.ParseLine(lines[x]);
			}

			CSVObject csv = parser.Flush();

			if(csv == null)
			{
				csv = new CSVObject();
			}
			csv.AddHeaders("key", "en-us");

			string[] keyNames = Enum.GetNames(typeof(ELocKey));
			HashSet<string> knownNames = new HashSet<string>();
			for(int x = 0; x < csv.Count; x++)
			{
				knownNames.Add(csv[x][0]);
			}

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

		private void SaveCSV()
		{
			if(Provider.onlineState != OnlineState.Offline)
			{
				Provider.Checkout(Target.LocalizationCSV, CheckoutMode.Asset).Wait();
			}
			string path = AssetDatabase.GetAssetPath(Target.LocalizationCSV);
			File.WriteAllText(path, m_CSV.ToString());
		}
	}
}
