using UnityEngine;
using System.Collections;

public class ztestVector : MonoBehaviour {

	public Transform _caTarget;
	public GameObject _outObj;
	public Vector3 _v1;
	public GameObject _arrow;

	CreatCharacter enemySpawn ;

	// Use this for initialization
	void Start () {

		enemySpawn = (CreatCharacter)FindObjectOfType<CreatCharacter> ();

				Vector3 viewPos = GetComponent<Camera>().WorldToViewportPoint (_outObj.transform.position);
		viewPos = GetComponent<Camera>().ViewportToScreenPoint (viewPos);
		print (viewPos);
		viewPos.x -= GetComponent<Camera>().pixelWidth / 2;
		viewPos.y -= GetComponent<Camera>().pixelHeight / 2;

		_arrow.transform.localPosition = viewPos;
		_arrow.transform.localPosition = viewPos;

		}
	// Update is called once per frame
	void Update () {
		
		if (_outObj.GetComponent<Renderer>().isVisible) {
			Debug.Log ("in");
		}
	}
}
