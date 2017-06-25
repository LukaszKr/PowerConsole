namespace ProceduralLevel.PowerConsole.Logic
{
	public interface IPersistence
	{
		void WriteFile(string fileName, string data);
		string ReadFile(string fileName);
	}
}
