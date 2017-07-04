using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class ValueParser
	{
		private Dictionary <Type, ValueParserDelegate> m_Parsers;

		public delegate object ValueParserDelegate(string rawValue);

		public ValueParser()
		{
			m_Parsers = new Dictionary<Type, ValueParserDelegate>();

			AddParser<bool>(BoolParser);
			AddParser<byte>(ByteParser);
			AddParser<short>(ShortParser);
			AddParser<ushort>(UShortParser);
			AddParser<int>(IntParser);
			AddParser<uint>(UIntParser);
			AddParser<long>(LongParser);
			AddParser<float>(FloatParser);
			AddParser<double>(DoubleParser);
			AddParser<string>(StringParser);
		}

		public void AddParser<T>(ValueParserDelegate parser)
		{
			Type type = typeof(T);
			m_Parsers[type] = parser;
		}

		public void CreateEnumParser<T>() where T: struct, IConvertible
		{
			AddParser<T>(GenerateEnumParser<T>());
		}

		public ValueParserDelegate GetParser<T>()
		{
			Type type = typeof(T);
			return GetParser(type);
		}

		public ValueParserDelegate GetParser(Type type)
		{
			ValueParserDelegate parser;
			if(!m_Parsers.TryGetValue(type, out parser))
			{
				if(type.IsEnum)
				{
					parser = GenerateEnumParser(type);
					m_Parsers[type] = parser;
				}
			}
			return parser;
		}

		public object Parse<T>(string rawValue)
		{
			return Parse(typeof(T), rawValue);
		}

		public object Parse(Type type, string rawValue)
		{
			ValueParserDelegate parser = GetParser(type);
			if(parser != null)
			{
				try
				{
					return parser(rawValue);
				}
				catch
				{
					throw new InvalidValueFormatException(type, rawValue);
				}
			}
			throw new MissingValueParserException(type, rawValue);
		}

		#region Simple Types Parsers
		private static object BoolParser(string rawValue)
		{
			if(rawValue == "1")
			{
				return true;
			}
			else if(rawValue == "0")
			{
				return false;
			}
			return bool.Parse(rawValue);
		}

		private static object ByteParser(string rawValue)
		{
			return byte.Parse(rawValue);
		}

		private static object ShortParser(string rawValue)
		{
			return short.Parse(rawValue);
		}

		private static object UShortParser(string rawValue)
		{
			return ushort.Parse(rawValue);
		}

		private static object IntParser(string rawValue)
		{
			return int.Parse(rawValue);
		}

		private static object UIntParser(string rawValue)
		{
			return uint.Parse(rawValue);
		}

		private static object LongParser(string rawValue)
		{
			return long.Parse(rawValue);
		}

		private static object FloatParser(string rawValue)
		{
			return float.Parse(rawValue);
		}

		private static object DoubleParser(string rawValue)
		{
			return double.Parse(rawValue);
		}

		private static object StringParser(string rawValue)
		{
			return rawValue;
		}
		#endregion

		#region Parser Generator
		private static ValueParserDelegate GenerateEnumParser<T>() where T: struct, IConvertible
		{
			return GenerateEnumParser(typeof(T));
		}

		private static ValueParserDelegate GenerateEnumParser(Type type)
		{
			string[] rawValues = Enum.GetNames(type);
			//this is because IsDefined is caseSensitive and it's needed to filter out numbers without enum value
			HashSet<string> values = new HashSet<string>();
			for(int x = 0; x < rawValues.Length; x++)
			{
				values.Add(rawValues[x].ToLower());
			}

			return (string rawValue) =>
			{
				if(!Enum.IsDefined(type, rawValue) && !values.Contains(rawValue.ToLower()))
				{
					throw new ArgumentException();
				}
				return Enum.Parse(type, rawValue, true);
			};
		}
		#endregion
	}
}
