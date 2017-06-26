namespace ProceduralLevel.PowerConsole.Logic
{
	public class LocalizationManager
	{
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
	}
}
