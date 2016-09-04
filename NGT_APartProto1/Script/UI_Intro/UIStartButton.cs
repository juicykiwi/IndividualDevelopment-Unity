using UnityEngine;
using System.Collections;

public class UIStartButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnClick()
	{
		StartCoroutine ("Wait");

	}

	IEnumerator Wait(){
		transform.FindChild ("Loading").gameObject.SetActive (true);
		yield return new WaitForSeconds(0.7f);
		Application.LoadLevel ("2_battle");

		//AsyncOperation async = Application.LoadLevelAsync ("2_battle");
		//yield return async;
		//while (!async.isDone) {
		//	transform.FindChild ("Loading").gameObject.SetActive (true);	
	}
}
