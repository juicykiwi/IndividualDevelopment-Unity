using UnityEngine;
using System;
using System.Collections;

public class SceneManager : ManagerBase<SceneManager> {

	public SceneController _scene = null;

	public Action<SceneType> _actionSceneLoaded = null;
	public Action<SceneType> _actionSceneClosed = null;

	// Method
}
