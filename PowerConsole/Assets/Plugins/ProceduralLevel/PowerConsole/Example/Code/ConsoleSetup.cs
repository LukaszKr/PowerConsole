using ProceduralLevel.PowerConsole.Logic;
using ProceduralLevel.PowerConsole.View;
using UnityEngine;

public class ConsoleSetup: MonoBehaviour
{
	public PrefabManager Manager;
	public ConsoleView ConsoleView;

	public void Start()
	{
		ConsoleView.Console.AddCommand(new SpawnCommand(Manager, ConsoleView.Console, "spawn", "Spawn a prefab"));

		ConsoleMessagesPanel messages = ConsoleView.Messages;
		messages.AddMessage(new Message(EMessageType.Info, 
			"You can hide or show console with '~'"));
		messages.AddMessage(new Message(EMessageType.Info, 
			"To autocomplete or get next hint, press 'tab'"));
		messages.AddMessage(new Message(EMessageType.Info,
			"To get list of all commands type 'list'"));
		messages.AddMessage(new Message(EMessageType.Info, 
			"You can input parameters out of order by typing name=value, for example 'spawn cube y=2'"));
		messages.AddMessage(new Message(EMessageType.Info, 
			"Up to "+HistoryModule.EXECUTION_HISTORY_LIMIT+" last commands are saved, you can iterate over them with up and down arrows"));
		messages.AddMessage(new Message(EMessageType.Info,
			"You can also create new commands by recording a macro. To do this type 'macro record [name]' then all comands you need. " +
			"When you are done, just type 'macro save'.From now on you can use this macro just like any other command. " +
			"You can also remove them by typing 'macro remove [name]'"));
	}
}
