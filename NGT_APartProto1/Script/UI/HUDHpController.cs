using UnityEngine;
using System.Collections;

public class HUDHpController : MonoBehaviour {

	public GameObject _HpFilledSpritePrefab = null;

	protected GameObject _hugObject = null;
	protected UISprite _hpBar = null;
	protected UIFollowTargetInProto _uiFollowTarget = null;

	// Use this for initialization
	void Start () {
		// We need the HUD object to know where in the hierarchy to put the element
		if (HUDRoot.go == null)
		{
			GameObject.Destroy(this);
			return;
		}

		_hugObject = NGUITools.AddChild(HUDRoot.go, _HpFilledSpritePrefab);
		_hugObject.transform.localScale = _HpFilledSpritePrefab.transform.localScale;
		_hpBar = _hugObject.GetComponentInChildren<UISprite>();

		_uiFollowTarget = _hugObject.AddComponent<UIFollowTargetInProto>();
		_uiFollowTarget.target = transform;
	}
	
	// Update is called once per frame
	void Update () {
		_hpBar.enabled = _uiFollowTarget.IsVisible();
	}

	void OnDestroy()
	{
		NGUITools.Destroy(_hugObject);
	}

	public void SetHpBarFillAmount(float amount)
	{
		amount = Mathf.Min (1.0f, amount);
		amount = Mathf.Max (0.0f, amount);

		_hpBar.fillAmount = amount;
	}
}
