using System.Collections;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class DelayOption: AConsoleCommand
	{
		public DelayOption(ConsoleInstance console)
			: base(console, ELocKey.OptDelayName, ELocKey.OptDelayDesc, true)
		{
		}

		public IEnumerator Command(float seconds)
		{
			yield return new UnityEngine.WaitForSeconds(seconds);
			yield return null;
		}
	}
}
