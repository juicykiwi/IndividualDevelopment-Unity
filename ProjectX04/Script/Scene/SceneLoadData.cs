using UnityEngine;
using System.Collections;

public class SceneLoadData : SceneController {

	static SceneLoadData _sceneLoadDataInstance = null;
	public static new SceneLoadData instance { get { return _sceneLoadDataInstance; } }
	
	public override SceneType sceneType { get { return SceneType.LoadData; } }

	bool _isDoNextScene = false;
	
	// Method
	
	protected override void Awake () {
		_sceneLoadDataInstance = this;
	}

	protected override void StartScene()
	{
		base.StartScene();

		LoadManagerController();
	}

	protected override void EndScene ()
	{
		base.EndScene();
	}

	void FixedUpdate () {
		if (_isDoNextScene == false && _sceneRunTime > 3f)
		{
			_isDoNextScene = true;
			DoNextScene();
		}
	}
	
	public void DoNextScene()
	{
		LoadNextSecne(SceneType.SelectStage);
	}
}
