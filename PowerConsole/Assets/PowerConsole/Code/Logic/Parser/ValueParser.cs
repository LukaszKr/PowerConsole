﻿using System;
using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class ValueParser
	{
		private Dictionary <Type, ValueParserDelegate> m_Parsers;

		public delegate object ValueParserDelegate(string rawValue);

		private bool m_CreateMissingEnumParsers;

		public ValueParser(bool createMissingEnumParsers = false)
		{
			m_Parsers = new Dictionary<Type, ValueParserDelegate>();
			m_CreateMissingEnumParsers = createMissingEnumParsers;

			AddParser<byte>(ByteParser);
			AddParser<short>(ShortParser);
			AddParser<int>(IntParser);
			AddParser<long>(LongParser);
			AddParser<float>(FloatParser);
			AddParser<double>(DoubleParser);
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
			m_Parsers.TryGetValue(type, out parser);
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
					throw new ValueParserException(type, rawValue);
				}
			}
			throw new MissingValueParserException(type, rawValue);
		}

		#region Simple Types Parsers
		public object ByteParser(string rawValue)
		{
			return byte.Parse(rawValue);
		}

		public object ShortParser(string rawValue)
		{
			return short.Parse(rawValue);
		}

		public object IntParser(string rawValue)
		{
			return int.Parse(rawValue);
		}

		public object LongParser(string rawValue)
		{
			return long.Parse(rawValue);
		}

		public object FloatParser(string rawValue)
		{
			return float.Parse(rawValue);
		}

		public object DoubleParser(string rawValue)
		{
			return double.Parse(rawValue);
		}
		#endregion

		#region Parser Generator
		private ValueParserDelegate GenerateEnumParser<T>() where T: struct, IConvertible
		{
			return (string rawValue) =>
			{
				return Enum.Parse(typeof(T), rawValue);
			};
		}
		#endregion
	}
}
