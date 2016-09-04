using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICoinPanel : MonoBehaviour {

	Text _coinCountText = null;

	void Awake()
	{
		_coinCountText = GetComponentInChildren<Text>();

		UIManager.instance._changedCoinCountAction += OnChangedCoinCount;
	}

	void OnDestroy()
	{
		if (UIManager.instance)
		{
			UIManager.instance._changedCoinCountAction -= OnChangedCoinCount;
		}
	}

	void OnChangedCoinCount(int coinCount)
	{
		_coinCountText.text = coinCount.ToString();
	}
}
