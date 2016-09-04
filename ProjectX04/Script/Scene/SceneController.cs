using UnityEngine;
using System.Collections;

public enum SceneType
{
	None,

	// dev

	FieldMaker,

	// game

	Logo,
	LoadData,
	SelectStage,
	GameStage,
}

public class SceneController : MonoBehaviour {

	static SceneController _instance = null;
	public static SceneController instance { get { return _instance; } }

	public virtual SceneType sceneType { get { return SceneType.None; } }

	public float _sceneRunTime = 0f;

	public Canvas _canvas = null;

	// Method

	protected virtual void Awake () {
		_instance = this;

		_canvas = FindObjectOfType<Canvas>();

		LoadManagerController();
	}

	// Use this for initialization
	void Start () {
		StartScene();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		_sceneRunTime += Time.deltaTime;
	}

	protected virtual void StartScene()
	{
		if (SceneManager.instance._actionSceneLoaded != null)
		{
			SceneManager.instance._actionSceneLoaded(sceneType);
		}
	}

	protected virtual void EndScene()
	{
		if (SceneManager.instance._actionSceneClosed != null)
		{
			SceneManager.instance._actionSceneClosed(sceneType);
		}
	}

	protected void LoadManagerController()
	{
        Transform parentTrans = ManagerController.instance.transform;

        SceneManager.CreateInstance(parentTrans);
        LoadDataManager.CreateInstance(parentTrans);
        FieldManager.CreateInstance(parentTrans);
        ChaManager.CreateInstance(parentTrans);
        TouchManager.CreateInstance(parentTrans);
        StageManager.CreateInstance(parentTrans);
        EventManager.CreateInstance(parentTrans);
        PathFinder.CreateInstance(parentTrans);
        PlayerPrefsManager.CreateInstance(parentTrans);
        UserInfoManager.CreateInstance(parentTrans);
        CameraManager.CreateInstance(parentTrans);
        UIManager.CreateInstance(parentTrans);
        KeyManager.CreateInstance(parentTrans);
        EffectManager.CreateInstance(parentTrans);
	}

	protected void LoadNextSecne(SceneType sceneType)
	{
		EndScene();

		string nextSceneName = "";

		switch (sceneType)
		{
		case SceneType.FieldMaker:
			nextSceneName = "dev_00_FieldMaker";
			break;
		case SceneType.Logo:
			nextSceneName = "Logo";
			break;
		case SceneType.LoadData:
			nextSceneName = "InitLoadData";
			break;
		case SceneType.SelectStage:
			nextSceneName = "01_SelectStage";
			break;
		case SceneType.GameStage:
			nextSceneName = "02_StageMain2D";
			break;
		default:
			return;
		}

		Application.LoadLevel(nextSceneName);
	}
}
