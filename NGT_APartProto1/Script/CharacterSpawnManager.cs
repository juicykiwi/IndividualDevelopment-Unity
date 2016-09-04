using UnityEngine;
using System.Collections;

public class CharacterSpawnManager : MonoBehaviour {

	public ArrayList characterSpawnPosList;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Init()
	{
		InitCharacterSpawnPos();
	}

	public void InitCharacterSpawnPos()
	{
		characterSpawnPosList = new ArrayList();

		CharacterSpawnInfo[] spawnInfo = GetComponentsInChildren<CharacterSpawnInfo>();
		for (int index = 0; index < spawnInfo.Length; ++index)
		{
			characterSpawnPosList.Add(spawnInfo[index].transform.position);
		}
	}
}
