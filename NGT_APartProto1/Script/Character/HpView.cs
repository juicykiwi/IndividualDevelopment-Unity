using UnityEngine;
using System.Collections;

public class HpView : MonoBehaviour {

	BaseCharacter _baseCharacter;

	Vector3 _vecScale;

	// Use this for initialization
	void Start () {
		_baseCharacter = transform.parent.GetComponent<BaseCharacter>();
		_vecScale = new Vector3(0.3f, 2.0f, 0.3f);

		if (_baseCharacter._battleSide == BattleSide.B) {
			//Material lma = this.GetComponent<Material>();
			//lma.color = Color.green;
			this.GetComponent<Renderer>().material.color = Color.green;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (_baseCharacter) {
			float hPercent = (float)_baseCharacter.getStat()._hpCurrent / (float)_baseCharacter.getStat()._hpMax;

			if( 0.0f < hPercent)
			{
				_vecScale.y = 2.0f*hPercent;
				transform.localScale = _vecScale;
			}
			else
			{
				_vecScale.y = 0.0f;
				transform.localScale = _vecScale;
			}
		}
	}
}
