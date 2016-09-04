using UnityEngine;
using System.Collections;

public class SceneDevFieldMaker : SceneController {

	// Property
	static SceneDevFieldMaker _sceneDevFieldMakerInstance = null;
	public static new SceneDevFieldMaker instance { get { return _sceneDevFieldMakerInstance; } }

	public override SceneType sceneType { get { return SceneType.FieldMaker; } }

	// Method

	protected override void Awake () {
		base.Awake();

		_sceneDevFieldMakerInstance = this;
	}

	protected override void StartScene()
	{
		base.StartScene();
	}

	protected override void EndScene()
	{
		base.EndScene();
	}
}
