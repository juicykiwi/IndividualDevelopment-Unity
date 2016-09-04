using UnityEngine;
using System.Collections;

public class UIEnchantButton : MonoBehaviour {

	void Awake () {

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick()
	{
		SceneBattleSkillSelect.GetInstance().DoSkillEnchant();
	}
}
