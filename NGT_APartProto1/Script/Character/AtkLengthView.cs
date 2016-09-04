using UnityEngine;
using System.Collections;

public class AtkLengthView : MonoBehaviour {

	BaseCharacter _baseCharacter;

	// Use this for initialization
	void Start () {
		_baseCharacter = transform.parent.GetComponent<BaseCharacter>();
		return;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = new Vector3(_baseCharacter.getAttackRange()*2.0f, transform.localScale.y, _baseCharacter.getAttackRange()*2.0f);
		string log = string.Format ("{0}", _baseCharacter.getAttackRange ());
		Debug.Log(log);
	
	}
}
