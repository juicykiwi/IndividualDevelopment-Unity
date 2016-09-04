using UnityEngine;
using System.Collections;

public class UIUnitSelect : MonoBehaviour {

	public int _unitNum;
	public GameObject _unitSelect;
	public GameObject _selectButton;
	UIUnitSlot uiunit = null;


	// Use this for initialization
	void Start () {
		uiunit = (UIUnitSlot)FindObjectOfType<UIUnitSlot> ();
					}
	
	// Update is called once per frame
	void Update () 
	{
	/*	if (resetSlot._resetSlot==true) 
		{
			for (int i = 0; i < 4; i++) {
				Destroy (uiunit._unitSlot [i]);
				Destroy (_unitSelect);
					}
			resetSlot._resetSlot=false;
		}*/

	}


	void OnClick()
	{
		//Debug.Log ("Click");
		_selectButton.transform.localPosition = this.gameObject.transform.localPosition;   //커서 위치 설정
		for (int i = 0; i < 12; i++) {
			transform.parent.parent.FindChild("Description").GetChild(i).gameObject.SetActive(false);   //유닛 설명 끄기
				}
		transform.parent.parent.FindChild ("Description").GetChild (_unitNum - 2).gameObject.SetActive (true); //유닛 설명 켜기

		Destroy (transform.parent.FindChild ("CurrentObj").GetChild (0).gameObject);    //현재 선택오브젝트 복사 및 숨겨놓기
		uiunit._selectedUnit = Instantiate (gameObject) as GameObject;
		uiunit._selectedUnit.transform.parent = transform.parent.FindChild ("CurrentObj");
		//uiunit._selectedUnit.gameObject.SetActive (false);
		/*
		for (int i = 0; i < 4; i++) 
		{
			if (gameObject.name=="Unit_Summoner") {
				_unitSelect = Instantiate (gameObject) as GameObject;
				_unitSelect.transform.parent = transform.parent.parent.FindChild("SelectedSlot").GetChild(4);
				_unitSelect.transform.localPosition = Vector3.zero;
				uiunit._startUnitNum[0] = _unitNum;
				break;
			}
			if (uiunit._unitSlot[i]!=null) 
				{
				}
			else{
				_unitSelect = Instantiate (gameObject) as GameObject;
				uiunit._unitSlot[i] = (GameObject)_unitSelect;
				uiunit._unitSlot[i].transform.parent = transform.parent.parent.FindChild("SelectedSlot").GetChild(i);
				uiunit._unitSlot[i].transform.localPosition =Vector3.zero;
				_unitSelect.transform.localScale = Vector3.one;
				uiunit._startUnitNum[i+1] = _unitNum;
				break;
				}
		}*/
	}
}
