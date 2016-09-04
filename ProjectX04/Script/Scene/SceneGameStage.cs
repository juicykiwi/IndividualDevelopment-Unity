using UnityEngine;
using System.Collections;

public class SceneGameStage : SceneController {

	// Property

	static SceneGameStage _sceneGameStageInstance = null;
	public static new SceneGameStage instance { get { return _sceneGameStageInstance; } }
	
	public override SceneType sceneType { get { return SceneType.GameStage; } }
	
	// Method

	protected override void Awake () {
		base.Awake();
		
		_sceneGameStageInstance = this;
	}

	protected override void StartScene()
	{
		base.StartScene();

		Invoke("RequestStartStage", 0.1f);
	}
	
	protected override void EndScene()
	{
		base.EndScene();
	}

	public void StartSelectStageScene()
	{
		LoadNextSecne(SceneType.SelectStage);
	}
	
	void RequestStartStage()
	{
		StageManager.instance.StartStage(StageManager.instance._startStageIndex);
	}
}
