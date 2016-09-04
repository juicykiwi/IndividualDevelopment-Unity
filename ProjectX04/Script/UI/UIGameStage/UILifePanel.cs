using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UILifePanel : MonoBehaviour {

	public List<GameObject> _heartList = new List<GameObject>();

	// Method

	void Awake () {
		if (UIManager.instance)
		{
			UIManager.instance._changedUserHpAction += UpdateLife;
		}
	}

	void OnDestroy () {
		if (UIManager.instance)
		{
			UIManager.instance._changedUserHpAction -= UpdateLife;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateLife(ChaController user)
	{
		if (user == null)
			return;

		for (int i = 0; i < _heartList.Count; ++i)
		{
			if (i < user.stat._life)
			{
				_heartList[i].SetActive(true);
			}
			else
			{
				_heartList[i].SetActive(false);
			}
		}
	}
}
