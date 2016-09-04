using UnityEngine;
using System.Collections;

public class SceneLogo : SceneController {

	static SceneLogo _sceneLogoInstance = null;
	public static new SceneLogo instance { get { return _sceneLogoInstance; } }
	
	public override SceneType sceneType { get { return SceneType.Logo; } }

	public UILogoPanel _uiLogoPanel = null;

	// Method

	protected override void Awake () {
		// base.Awake();
		
		_sceneLogoInstance = this;

		_uiLogoPanel = FindObjectOfType<UILogoPanel>();
	}

	protected override void StartScene()
	{
		base.StartScene();
		
		_uiLogoPanel.StartLogo();
	}
	
	protected override void EndScene ()
	{
		base.EndScene();
	}

	public void DoNextScene()
	{
		LoadNextSecne(SceneType.LoadData);
	}
}
