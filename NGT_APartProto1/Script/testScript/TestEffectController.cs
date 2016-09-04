using UnityEngine;
using System.Collections;

public class TestEffectController : MonoBehaviour {

	public GameObject effect01;
	public GameObject effect02;
	public GameObject effect03;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void createEffect() {
		Vector3 pos = new Vector3(-5.0f, 1.0f, 0.0f);
		Instantiate(effect01, pos, transform.rotation);

		pos.x += 5.0f;
		Instantiate(effect02, pos, transform.rotation);

		pos.x += 5.0f;
		Instantiate(effect03, pos, transform.rotation);
	}
}
