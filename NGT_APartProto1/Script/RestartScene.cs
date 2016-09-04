using UnityEngine;
using System.Collections;

public class RestartScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		}

	void OnClick()
	{
		if (gameObject.name=="ButtonHome") {
			Application.LoadLevel ("0_HeroSelect");
			Destroy (GameObject.Find ("StartMemberList"));
				}
		else if (gameObject.name=="ButtonRestart") {
			Application.LoadLevel ("2_Battle");
				}


		}

}
