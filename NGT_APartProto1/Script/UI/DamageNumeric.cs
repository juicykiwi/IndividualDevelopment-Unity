using UnityEngine;
using System.Collections;

public enum NumericType
{
	NormalDamage,
	CriDamage,
	Heal,
}

public class DamageNumeric : MonoBehaviour {

	public GameObject _hudTextPrefab = null;
	public Color _textColorDamage = new Color(1.0f, 1.0f, 1.0f);
	public Color _textColorCri = new Color(1.0f, 1.0f, 1.0f);
	public Color _taxtColorHeal = new Color(1.0f, 1.0f, 1.0f);

	protected GameObject _hugObject = null;
	protected HUDText _hudText = null;
	protected UIFollowTargetInProto _uiFollowTarget = null;

	// Use this for initialization
	void Start () {
		// We need the HUD object to know where in the hierarchy to put the element
		if (HUDRoot.go == null)
		{
			GameObject.Destroy(this);
			return;
		}

		_hugObject = NGUITools.AddChild(HUDRoot.go, _hudTextPrefab);
		_hudText = _hugObject.GetComponentInChildren<HUDText>();

		_uiFollowTarget = _hugObject.AddComponent<UIFollowTargetInProto>();
		_uiFollowTarget.target = transform;
	}
	
	// Update is called once per frame
	void Update () {
		_hudText.enabled = _uiFollowTarget.IsVisible ();
	}

	void OnDestroy()
	{
		NGUITools.Destroy(_hugObject);
	}

	public void print(int damage, NumericType numericType)
	{
		string str = string.Format("{0}", damage);

		switch (numericType)
		{
		case NumericType.NormalDamage:
		{
			_hudText.Add (str, _textColorDamage, 18, 0.0f);
		}
			break;

		case NumericType.CriDamage:
		{
			_hudText.Add ("Critical!", _textColorCri, 15, 0.0f);
			_hudText.Add (str, _textColorCri, 42, 0.0f);
		}
			break;

		case NumericType.Heal:
		{
			_hudText.Add (str, _taxtColorHeal, 52, 0.0f);
		}
			break;
		}
	}
}
