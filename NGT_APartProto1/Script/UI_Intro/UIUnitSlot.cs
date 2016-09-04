using UnityEngine;
using System.Collections;

public class UIUnitSlot : MonoBehaviour {

	public GameObject[] _unitSlot = new GameObject[4];
	public int[] _startUnitNum = new int[5];
	public GameObject[] _unitList = new GameObject[10];
	public GameObject _selectedUnit = null;

	UIHeroSelect selectedHero ;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (this.gameObject);
	}
	void Start () {
		for (int i = 0; i < _unitSlot.Length; i++) {
			_unitSlot[i] = null;
					}
		selectedHero = (UIHeroSelect)FindObjectOfType<UIHeroSelect> ();
		if (selectedHero)
		{
			switch (selectedHero._selectedHero) {
			case UIHeroSelect.SelectedHero.none:
				break;
			case UIHeroSelect.SelectedHero.Knight:{
				Debug.Log ("knight");
				_startUnitNum[0] = 0;
				}
				break;
			case UIHeroSelect.SelectedHero.Cleric:{
				Debug.Log ("Cleric");
				_startUnitNum[0] = 1;
				}
				break;
			default:
				break;
			}
		}

		//resetSlot = (UIResetButton)FindObjectOfType<UIResetButton> ();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
