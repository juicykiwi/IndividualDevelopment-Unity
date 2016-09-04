using UnityEngine;
using System.Collections;

public class SceneBase : MonoBehaviour {

	public static SceneBase GetInstance() { return _instance; }
	protected static SceneBase _instance = null;
	
	public string _nextSceneName = "";

	void Awake () {
		if (_instance == null)
		{
			_instance = this;
		}
		
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DoNextScene()
	{
		Application.LoadLevel(_nextSceneName);
	}
}
