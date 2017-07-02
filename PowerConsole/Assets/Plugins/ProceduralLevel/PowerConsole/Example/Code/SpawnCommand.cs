using ProceduralLevel.PowerConsole.Logic;
using System.Reflection;
using UnityEngine;

public class SpawnCommand: AConsoleCommand
{
	private PrefabManager m_Manager;
	/// <param name="console">Instance of console that this will be attached to</param>
	/// <param name="name">This is the name that you will use in console. It will be turned lowerCase but input is case insensitive</param>
	/// <param name="description">Short description of what this command does</param>
	/// <param name="isOption">If it's an option set to true. Options are called just like commands but with '-' prefix</param>
	public SpawnCommand(PrefabManager manager, ConsoleInstance console, string name, string description, bool isOption = false) 
		: base(console, name, description, isOption)
	{
		m_Manager = manager;
	}

	/// <summary>
	/// You can provide custom hints for parameter by extending ADynamicHint or ACollectionHint. Hints should be singletones whenever possible.
	/// </summary>
	/// <param name="manager">Keeps track of all common hints</param>
	/// <param name="parameterIndex">Index of parameter that you should provide hint for.</param>
	public override AHint GetHintFor(HintManager manager, int parameterIndex)
	{
		return base.GetHintFor(manager, parameterIndex);
	}

	/// <summary>
	/// If you want to use different names than 'Command' for method that will get executed, override this method like shown below.
	/// </summary>
	public override MethodInfo GetCommandMethod()
	{
		return GetMethodInfo(DEFAULT_COMMAND_NAME);
	}

	/// <param name="prefabName">Primitive types (int, bool, strings etc.) and Enums are supported as command parameters.</param>
	/// <returns>Command method has to return Message - it can be null</returns>
	public Message Command(EPrefabName prefabName = EPrefabName.Cube, float x = 0f, float y = 0f, float z = 0f)
	{
		GameObject instance = m_Manager.SpawnPrefab(prefabName);
		if(instance == null)
		{
			return new Message(EMessageType.Error, string.Format("Could not find prefab with name '{0}'", prefabName.ToString()));
		}
		instance.transform.position = new Vector3(x, y, z);
		return new Message(EMessageType.Success, string.Format("Succesfully spawned '{0}'", prefabName.ToString()));
		//if you don't want to print anything to console after execution, return null
		//return null;
	}
}


