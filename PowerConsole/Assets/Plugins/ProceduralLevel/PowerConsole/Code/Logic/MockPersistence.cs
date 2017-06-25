namespace ProceduralLevel.PowerConsole.Logic
{
	public class MockPersistence: IPersistence
	{
		public string ReadFile(string fileName)
		{
			return string.Empty;
		}

		public void WriteFile(string fileName, string data)
		{
		}
	}
}
