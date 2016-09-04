using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabDataDict<T> where T : MonoBehaviour
{
	Dictionary<string, GameObject> _prefabDict = new Dictionary<string, GameObject>();

    // Method

	public void LoadPrefabAll()
	{
		T[] prefabList = Resources.LoadAll<T>("");

		foreach (T prefab in prefabList)
		{
			if (prefab == null)
				continue;

			_prefabDict.Add(prefab.gameObject.name, prefab.gameObject);
		}
	}

	public void Clear()
	{
		_prefabDict.Clear();
	}

	public List<GameObject> GetPrefabList()
	{
		List<GameObject> prefabList = new List<GameObject>();

		prefabList.AddRange(_prefabDict.Values);
		return prefabList;
	}

	public GameObject GetPrefabByName(string name)
	{
		if (_prefabDict.ContainsKey(name) == false)
			return null;

		return _prefabDict[name];
	}
}
