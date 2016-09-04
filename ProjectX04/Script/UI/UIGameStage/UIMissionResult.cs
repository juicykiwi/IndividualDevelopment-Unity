using UnityEngine;
using System.Collections;

public class UIMissionResult : MonoBehaviour {

	public GameObject _backPanel = null;
	public GameObject _successText = null;
	public GameObject _failText = null;

	// Method

	void Awake () {
		_backPanel.SetActive(false);
		_successText.SetActive(false);
		_failText.SetActive(false);
	}

	// Use this for initialization
	void Start () {
		if (EventManager.instance != null)
		{
			EventManager.instance._actionSuccessMission += OnMisionSuccess;
			EventManager.instance._actionFailMission += OnMisionFail;
		}
		
	}

	void OnDestroy () {
		if (EventManager.instance != null)
		{
			EventManager.instance._actionSuccessMission -= OnMisionSuccess;
			EventManager.instance._actionFailMission -= OnMisionFail;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMisionSuccess()
	{
		_backPanel.SetActive(true);
		_successText.SetActive(true);
	}

	public void OnMisionFail()
	{
		_backPanel.SetActive(true);
		_failText.SetActive(true);
	}
}
