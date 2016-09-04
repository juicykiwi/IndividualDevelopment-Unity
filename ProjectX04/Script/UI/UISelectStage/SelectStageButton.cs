using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectStageButton : MonoBehaviour {

	// Property

	public int _stageIndex = 0;

	public Text _text = null;
	public GameObject _lockObj = null;

	public bool _isLock = true;

	// Method

	void Awake () {
		_text.text = string.Format("Stage\n{0}", _stageIndex);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetLockButton(bool isLock)
	{
		_isLock = isLock;
		_lockObj.SetActive(isLock);
	}

	public void OnClickButton()
	{
		if (_isLock == true)
			return;

		SceneSelectStage.instance.StartStage(_stageIndex);
	}
}
