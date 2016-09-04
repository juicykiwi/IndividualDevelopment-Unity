using UnityEngine;
using System.Collections;

public class UIHeroSelect : MonoBehaviour {

	public enum SelectedHero {none, Knight, Cleric} ;

	public Camera MainCamera;
	public float cameraSpeed;
	public GameObject Hero1;
	public GameObject Hero2;
	public GameObject _selectButton;
	public SelectedHero _selectedHero = SelectedHero.none;

	UIHeroSelectAni selectAni = null;
	UIHeroSelectButton selectButton = null;

	void Awake(){
		DontDestroyOnLoad(transform.gameObject);
	}

	// Use this for initialization
	void Start () {
		selectButton = (UIHeroSelectButton)FindObjectOfType<UIHeroSelectButton> ();

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp(0)){
			Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
			rayCasting(ray);
			if (selectButton.GetComponent<UIHeroSelectButton> ()._SetBool) {
				if (_selectedHero!=SelectedHero.none) {
					Application.LoadLevel("0_SkillSelect");
					StartCoroutine("DestroyObj");
				}
			}

			
		}


	}
	
	void rayCasting(Ray ray)
	{
		RaycastHit hitObj;
		if (Physics.Raycast (ray, out hitObj, Mathf.Infinity)) 
		{
			if(hitObj.transform.name.Equals("KnightPrefab"))
			{
				selectAni = (UIHeroSelectAni)hitObj.transform.GetComponent<UIHeroSelectAni>();
				if(null != selectAni)
				{
					_selectedHero = SelectedHero.Knight;
					Hero1.transform.FindChild("Ef_Aura_2_loop").gameObject.SetActive(true);
					//Hero2.transform.FindChild("Ef_Aura_2_loop").gameObject.SetActive(false);
					selectAni._animator.SetBool("Idle",false);
					selectAni._animator.SetBool("Attack",true);
					//StartCoroutine("CameraMoveLeft");
					StartCoroutine("Wait");
				}
			}
			else if(hitObj.transform.name.Equals("ClericPrefab"))
			{
			selectAni = (UIHeroSelectAni)hitObj.transform.GetComponent<UIHeroSelectAni>();
				if(null != selectAni)
				{
					_selectedHero = SelectedHero.Cleric;
					Hero2.transform.FindChild("Ef_Aura_2_loop").gameObject.SetActive(true);
					Hero1.transform.FindChild("Ef_Aura_2_loop").gameObject.SetActive(false);
					selectAni._animator.SetBool("Idle",false);
					selectAni._animator.SetBool("Attack",true);
					StartCoroutine("CameraMoveRight");
					StartCoroutine("Wait");
				}
			}
		}
	}

	IEnumerator Wait(){
		yield return new WaitForSeconds(0.5f);
		selectAni._animator.SetBool("Attack",false);
		selectAni._animator.SetBool ("Idle", true);
	}
	IEnumerator CameraMoveLeft(){
		float currentSpeed = cameraSpeed;
		_selectButton.gameObject.SetActive (false);
		while (MainCamera.transform.localPosition.x > Hero1.transform.localPosition.x) {
			MainCamera.transform.localPosition -= new Vector3 (1f, 0, 0) * Time.deltaTime * cameraSpeed;
			yield return new WaitForSeconds (0.02f);
			cameraSpeed *= 0.9f;
			if (Time.deltaTime * cameraSpeed < 0.02f) {
				break;
			}
		}
		cameraSpeed = currentSpeed;
		yield return new WaitForSeconds (0.1f);
		_selectButton.gameObject.SetActive (true);
	}

	IEnumerator CameraMoveRight(){
		float currentSpeed = cameraSpeed;
		_selectButton.gameObject.SetActive (false);
		while (MainCamera.transform.localPosition.x < Hero2.transform.localPosition.x) {
			MainCamera.transform.localPosition += new Vector3 (1f, 0, 0) * Time.deltaTime * cameraSpeed;
			yield return new WaitForSeconds (0.02f);
			cameraSpeed *= 0.9f;
			if (Time.deltaTime * cameraSpeed < 0.02f) {
				break;
			}
		}
		cameraSpeed = currentSpeed;
		yield return new WaitForSeconds (0.1f);
		_selectButton.gameObject.SetActive (true);
	}

	IEnumerator DestroyObj(){
		yield return new WaitForSeconds (0.5f);
		Destroy (this.gameObject);
		}


	
}
