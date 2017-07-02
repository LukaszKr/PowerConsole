using ProceduralLevel.Serialization.CSV;
using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class LocalizationManager
	{
		private string[] ENUM_NAMES = Enum.GetNames(typeof(ELocKey));

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
				int keyCol = csv.Header.IndexOf(KEY);
				int langCol = csv.Header.IndexOf(lang);
				if(langCol < 0 || keyCol < 0)
				{
					throw new Exception("Could not find required columns");
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
			string strKey = ENUM_NAMES[(int)key];
			string value;
			m_Translations.TryGetValue(strKey, out value);
			return value?? strKey.ToLower();
		}

		public string Get(ELocKey key, params object[] args)
		{
			string locKey = Get(key);
			//for(int x = 0; x < args.Length; x++)
			//{
				//locKey += " - {"+x+"}";
			//}
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

		public string Get(NotEnoughtArgumentsException e)
		{
			return Get(ELocKey.LogicQueryNotEnoughtArguments, e.Required, e.Parameters.Count);
		}

		public string Get(NamedArgumentNotFoundException e)
		{
			return Get(ELocKey.LogicQueryNamedArgumentNotFound, e.Name);
		}

		public string Get(TooManyArgumentsException e)
		{
			return Get(ELocKey.LogicQueryTooManyArguments, e.Received, e.Expected);
		}

		public string Get(MissingValueParserException e)
		{
			return Get(ELocKey.LogicParsingMissingParser, e.RawValue, e.ExpectedType.Name);
		}

		public string Get(InvalidValueFormatException e)
		{
			return Get(ELocKey.LogicParsingInvalidFormat, e.RawValue, e.ExpectedType.Name);
		}

		public string Get(QueryParserException e)
		{
			return Get(ELocKey.LogicParserError, 
				e.ErrorCode, string.Format("['{0}', Position: {1}]", e.Token.Value, e.Token.Column));
		}
	}
}
