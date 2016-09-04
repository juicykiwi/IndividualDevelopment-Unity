using UnityEngine;
using System.Collections;

public class UIUseSkillResetButton : MonoBehaviour {

	protected SkillSlotController _skillSlotController = null;

	// Use this for initialization
	void Start () {
		_skillSlotController = FindObjectOfType<SkillSlotController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick()
	{
		_skillSlotController.ResetUseSkillList();
	}
}
