using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITopBarPanel : MonoBehaviour {

	[SerializeField] UILifePanel _uiLifePanel = null;
	[SerializeField] UICoinPanel _uiCoinPanel = null;
	[SerializeField] Button _uiExitButton = null;
	[SerializeField] UIGameMenu _uiGameMenu = null;

	public void OnActiveGameMenu()
	{
		if (_uiGameMenu == null)
			return;

		_uiGameMenu.gameObject.SetActive(true);
	}
}
