using System.Collections.Generic;

namespace ProceduralLevel.PowerConsole.Logic
{
	public class ListCommand: AConsoleCommand
	{
		public ListCommand(ConsoleInstance console, bool isOption = false) 
			: base(console, ELocKey.CmdListName, ELocKey.CmdListDesc, isOption)
		{
		}
		
		public Message Command(EListMode mode = EListMode.Commands)
		{
			string list = "";
			IEnumerator<AConsoleCommand> commands = Console.GetCommandEnumerator();
			while(commands.MoveNext())
			{
				AConsoleCommand command = commands.Current;
				bool isMacro = command is MacroPlayerCommand;
				switch(mode)
				{
					case EListMode.Commands:
						if(!command.IsOption && !isMacro)
						{
							list += command.Name+", ";
						}
						break;
					case EListMode.Options:
						if(command.IsOption)
						{
							list += command.Name+", ";
						}
						break;
					case EListMode.Macro:
						if(isMacro)
						{
							list += command.Name+", ";
						}
						break;
					case EListMode.All:
						if(command.IsOption)
						{
							list += ParserConst.OPTION+command.Name+", ";
						}
						else
						{
							list += command.Name+", ";
						}
						break;
				}
			}

			return new Message(EMessageType.Normal, list);
		}
	}
}
