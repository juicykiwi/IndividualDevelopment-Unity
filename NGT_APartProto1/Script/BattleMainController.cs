using UnityEngine;
using System.Collections;

public class BattleMainController : MonoBehaviour {

	public enum BattleState
	{

	};

	private CharacterManager characterManager;
	private CharacterSpawnManager characterSpawnManager;

	public GameObject ATeam;
	public GameObject BTeam;

	void Awake()
	{
		//Application.LoadLevelAdditive("battleField");
		//Application.LoadLevelAdditive("battleUI");

		// Init CharacterManager
		characterManager = (CharacterManager)FindObjectOfType<CharacterManager>();
		if (characterManager == null)
			Debug.LogError("characterManager is null!");

		// Init CharacterSpawnManger
		characterSpawnManager = (CharacterSpawnManager)FindObjectOfType<CharacterSpawnManager>();
		if (characterSpawnManager == null)
			Debug.LogError("CharacterSpawnManager is null!");
	}

	// Use this for initialization
	void Start () {
		characterSpawnManager.Init();

		InitBattle();
	}

	public void InitBattle()
	{
		// Init monster spawn info
		ArrayList characterSpawnPosList = characterSpawnManager.characterSpawnPosList;
		if (characterSpawnPosList == null)
		{
			Debug.LogError("characterSpawnPosList is null");
		}
	
		/*
		for (int index = 0; index < characterSpawnPosList.Count; ++index)
		{
			Vector3 spawnPos = (Vector3)characterSpawnPosList[index];
			string log = string.Format("Spawn pos - x:{0} / y:{1} / z:{2}", spawnPos.x, spawnPos.y, spawnPos.z);
			Debug.Log(log);

			GameObject character = characterManager.create(CharacterManager.CharacterType.Cube);
			if (character)
			{
				character.transform.position = spawnPos;
			}
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

