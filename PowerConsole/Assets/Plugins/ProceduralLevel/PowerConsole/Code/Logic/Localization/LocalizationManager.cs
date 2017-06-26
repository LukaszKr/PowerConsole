using System;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class LocalizationManager
	{
		private const string ENUM_FORMAT = "Gen{0}{1}";

		public string Get(ELocKey key)
		{
			return key.ToString();
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
