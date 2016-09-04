using UnityEngine;
using System.Collections;

public class UIGameMenu : MonoBehaviour {

	void Start()
	{
		gameObject.SetActive(false);
	}

	public void OnContinue()
	{
		gameObject.SetActive(false);
	}

	public void OnReturnMain()
	{
		StageManager.instance.FailStageClear();
	}
}
