using UnityEngine;
using System.Collections;

public class UINextScene_battleMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPress()
	{
	}
	
	void OnClick()
	{
		if (SceneBattleSkillSelect.GetInstance())
		{
			SceneBattleSkillSelect.GetInstance().DoNextScene();
		}
	}
	
	void OnHover()
	{
	}
	
	void OnMouseOver()
	{
	}
}
