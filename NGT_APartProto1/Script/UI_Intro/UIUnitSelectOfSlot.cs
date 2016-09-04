using UnityEngine;
using System.Collections;

public class UIUnitSelectOfSlot : MonoBehaviour {


	public GameObject selectedUnit = null;
	public GameObject emptyObj = null;
	public int slotIndex = 0;
	UIUnitSlot uiuSlot;
	UIUnitSelect uiuSelect;
	UIUnitSlotReset resetSlot ;

	// Use this for initialization
	void Start () {
		uiuSlot = (UIUnitSlot)FindObjectOfType<UIUnitSlot> ();
		uiuSelect = (UIUnitSelect)FindObjectOfType<UIUnitSelect> ();
		resetSlot = (UIUnitSlotReset)FindObjectOfType<UIUnitSlotReset> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (resetSlot._resetSlot == true) {				//빈오브젝트(empty) 아래에 선택 유닛 표시하기
			for (int i = 0; i < 4; i++) {
				Destroy(transform.parent.GetChild(i).GetChild(0).gameObject); //리셋버튼 누를때마다 빈오브젝트 갱신 및 하위 삭제
			}
			for (int i = 0; i < 4; i++) {
				emptyObj = Instantiate(transform.parent.FindChild("empty").gameObject) as GameObject;
				emptyObj.transform.parent = transform.parent.GetChild(i);
				emptyObj.transform.localPosition = Vector3.zero;
				emptyObj.transform.localScale = Vector3.one;

			}

			selectedUnit = null;
			resetSlot._resetSlot = false;

		}
	
	}

	void OnClick(){
		if (uiuSlot._selectedUnit!=null) {
			//if (this.selectedUnit!=null) {
				//Destroy(transform.FindChild("empty").GetChild(0).gameObject);
			//}
			selectedUnit = Instantiate( uiuSlot._selectedUnit) as GameObject;   //선택된 유닛 슬롯으로 복사하기
			selectedUnit.transform.parent = transform.GetChild(0);
			selectedUnit.gameObject.SetActive(true);
			selectedUnit.transform.localPosition = Vector3.zero;
			selectedUnit.transform.localScale = Vector3.one;
			Destroy(selectedUnit.GetComponent<BoxCollider>());
			Debug.Log("zzz");
			uiuSlot._startUnitNum[slotIndex+1] = (int)selectedUnit.GetComponent<UIUnitSelect>()._unitNum;
		}
	}
}

