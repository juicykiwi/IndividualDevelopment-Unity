using UnityEngine;
using System.Collections;

public class ResetClearStageButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick()
	{
		UserInfoManager.instance.ResetClearStageInfo();
		SceneSelectStage.instance.UpdateSelectStageButtonLock();
	}
}
