using UnityEngine;
using System.Collections;

public class UIHeroSelectButton : MonoBehaviour {

	public bool _SetBool = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick () {
		_SetBool = true;
		StartCoroutine ("SetBoolFalse");
	}

	IEnumerator SetBoolFalse(){
		yield return new WaitForSeconds (0.1f);
		_SetBool = false;
	}
}
