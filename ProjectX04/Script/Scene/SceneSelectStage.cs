using UnityEngine;
using System.Collections;

public class SceneSelectStage : SceneController {

	// Property
	
	static SceneSelectStage _sceneSelectStageInstance = null;
	public static new SceneSelectStage instance { get { return _sceneSelectStageInstance; } }
	
	public override SceneType sceneType { get { return SceneType.SelectStage; } }

	SelectStageButton[] _selectStageButtons = null;
	
	// Method

	protected override void Awake () {
		base.Awake();
		
		_sceneSelectStageInstance = this;

		_selectStageButtons = _canvas.GetComponentsInChildren<SelectStageButton>();
	}

	protected override void StartScene()
	{
		base.StartScene();

		UpdateSelectStageButtonLock();
	}
	
	protected override void EndScene ()
	{
		base.EndScene();
	}

	public void StartStage(int stageLevel)
	{
		if (StageManager.instance.IsStartStage(stageLevel) == false)
		{
			Debug.Log("Fail : SceneController.StartStage() - Not start stage.");
			return;
		}
		
		StageManager.instance._startStageIndex = stageLevel;
		LoadNextSecne(SceneType.GameStage);
	}

	public void UpdateSelectStageButtonLock()
	{
		UserInfoData userInfo = UserInfoManager.instance.GetUserInfoData();
		
		foreach (SelectStageButton button in _selectStageButtons)
		{
			if (button == null)
				continue;
			
			bool isLock = (button._stageIndex > (userInfo._lastClearStage + 1));
			button.SetLockButton(isLock);
		}
	}
}
