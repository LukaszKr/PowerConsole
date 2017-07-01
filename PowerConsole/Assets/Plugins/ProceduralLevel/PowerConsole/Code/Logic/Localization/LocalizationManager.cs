using ProceduralLevel.Serialization.CSV;
using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class LocalizationManager
	{
		private const string ENUM_FORMAT = "Gen{0}{1}";
		private const string KEY = "key";

		private Dictionary<string, string> m_Translations = new Dictionary<string, string>();

		public bool Load(string lang, string rawCSV)
		{
			CSVParser parser = new CSVParser();
			parser.Parse(rawCSV);
			CSVObject csv = parser.Flush();
			if(csv != null)
			{
				int keyCol = csv[0].IndexOf(KEY);
				int langCol = csv[0].IndexOf(lang);
				if(langCol < 0 || keyCol < 0)
				{
					return false;
				}

				for(int x = 0; x < csv.Count; x++)
				{
					CSVEntry entry = csv[x];
					string key = entry[keyCol];
					string value = entry[langCol];
					m_Translations[key] = value;
				}
			}

			return true;
		}

		public string Get(ELocKey key)
		{
			string value = key.ToString();
			m_Translations.TryGetValue(value, out value);
			return value?? key.ToString().ToLower();
		}

		public string Get(ELocKey key, params object[] args)
		{
			string locKey = Get(key);
			for(int x = 0; x < args.Length; x++)
			{
				locKey += " - {"+x+"}";
			}
			return string.Format(locKey, args);
		}

		public string GetEnum(object value)
		{
			if(value == null)
			{
				throw new ArgumentNullException();
			}
			Type type = value.GetType();
			if(!type.IsEnum)
			{
				throw new NotSupportedException();
			}
			return string.Format(ENUM_FORMAT, type.Name, value.ToString());
		}
	}
}
