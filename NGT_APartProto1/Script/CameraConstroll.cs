using UnityEngine;
using System.Collections;

public class CameraConstroll : MonoBehaviour {
//흔들기 기능 관련
	public Transform camTransform;
	public float shake = 0.5f;
	public float shakeAmount = 0.3f;
	public float decreaseFactor = 1.0f;
//zoom 기능 관련
	public Camera _zoomCamera;
	public float zoomSpeed = 2.5f;
	public float zoomTime = 0.6f;
//암전효과
	public GameObject shadowScreen;
//히어로 포커스 카메라
	public Camera _focusCamera;


	CharacterManager _characterManager = null;
	Vector3 originalPos;

	void Awake(){
		if (camTransform == null) {
			camTransform = GetComponent(typeof(Transform)) as Transform;		
		}
		_characterManager = (CharacterManager)FindObjectOfType<CharacterManager>();

	}

	void OnEnable(){
		originalPos = camTransform.localPosition;

	}
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	IEnumerator HeroFocus(){
		yield return new WaitForSeconds (0.05f);
		_focusCamera.gameObject.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		_focusCamera.gameObject.SetActive (false);
	}

	IEnumerator ZoomCamera(){
		float currentZoomTime = zoomTime;
		float currentZoomSpeed = zoomSpeed;
		_zoomCamera.transform.LookAt (_characterManager._playerSummoner.gameObject.transform) ;

		while (zoomTime>0) {
			yield return new WaitForSeconds (0.01f);
			_zoomCamera.transform.localPosition += new Vector3 (0f, zoomSpeed*0.03f, zoomSpeed);
			if (zoomSpeed>currentZoomSpeed*0.1f) {
				zoomSpeed *= 0.92f;
			}else{zoomSpeed *= 0.98f;}
			zoomTime -= Time.deltaTime ;
		}
		_zoomCamera.transform.localPosition = Vector3.zero;
		_zoomCamera.transform.localRotation = Quaternion.identity;
		zoomTime = currentZoomTime;
		zoomSpeed = currentZoomSpeed;
	}


	/*

	IEnumerator ShakeCamera(){
		float currentShake = shake;
		yield return new WaitForSeconds (0.6f);
		while (shake>0) {
			yield return new WaitForSeconds (0.015f);
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			shake -= Time.deltaTime * decreaseFactor;
				}
		shake = currentShake;
		camTransform.localPosition = originalPos;
	}*/

	IEnumerator ShadowScreen(){
		shadowScreen.gameObject.SetActive (true);
		yield return new WaitForSeconds (0.6f);
		shadowScreen.gameObject.SetActive (false);
	}

		/*if (shake > 0) {
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			shake -= Time.deltaTime * decreaseFactor;
		} else {
			shake = 0f;
			camTransform.localPosition = originalPos;
		}*/

}
