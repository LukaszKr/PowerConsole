using ProceduralLevel.PowerConsole.Logic;
using System.Collections;
using UnityEngine;

public class AsyncPrintCommand: AConsoleCommand
{
	public AsyncPrintCommand(ConsoleInstance console) : base(console, "asyncprint", "Prints a message after a given delay", false)
	{
	}

	public IEnumerator Command(string message, float delay = 0.5f)
	{
		yield return new WaitForSeconds(delay);
		yield return new Message(EMessageType.Success, message);
	}
}
