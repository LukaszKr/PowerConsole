using UnityEngine;

public enum EPrefabName
{
	Cube = 0,
	Sphere = 1
}

public class PrefabManager: MonoBehaviour
{
	public GameObject[] Prefabs;

	public GameObject SpawnPrefab(EPrefabName name)
	{
		int value = (int)name;
		if(value < 0 || value >= Prefabs.Length)
		{
			return null;
		}
		return Instantiate(Prefabs[value]);
	}
}
