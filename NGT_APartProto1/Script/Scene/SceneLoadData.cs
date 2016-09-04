using UnityEngine;
using System.Collections;

public class SceneLoadData : MonoBehaviour {

	public static SceneLoadData GetInstance() { return _instance; }
	protected static SceneLoadData _instance = null;
	
	public string _nextSceneName = "";

	void Awake () {
		if (_instance == null)
		{
			_instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		DoNextScene();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DoNextScene()
	{
		Application.LoadLevel(_nextSceneName);
	}
}
