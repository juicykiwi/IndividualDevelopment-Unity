using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EventManager : ManagerBase<EventManager> {

	// Delegate

	public Action<ChaController, Vector2, Vector2> _actionChaMoveStart = null;
	public Action<ChaController, Vector2, Vector2> _actionChaMoveFinish = null;
	public Action _actionTouchUserCha = null;
	public Action _actionCancelTouchUserCha = null;

	public Action _actionSuccessMission = null;
	public Action _actionFailMission = null;

	// Method

	protected override void Awake () {
		base.Awake();
	}

//	public override void ActionSceneLoaded(SceneType sceneType)
//	public override void ActionSceneClosed(SceneType sceneType)
}
