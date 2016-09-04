using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillSlotController : MonoBehaviour {

	public List<GameObject> _skillSlotList = new List<GameObject>();
	public List<GameObject> _skillSelectedSlotList = new List<GameObject>();

	public List<UIHaveSkillSlot> _uiHaveSkillSlotList = new List<UIHaveSkillSlot>();
	public List<UIUseSkillSlot> _uiUseSkillSlotList = new List<UIUseSkillSlot>();

	public UIHaveSkillSlot _selectedHaveSkillSlot = null;

	void Awake()
	{
		foreach (GameObject slotObject in _skillSlotList)
		{
			if (slotObject == null)
				continue;

			UIHaveSkillSlot haveSkillslot = slotObject.GetComponentInChildren<UIHaveSkillSlot>();
			if (haveSkillslot == null)
				continue;

			_uiHaveSkillSlotList.Add(haveSkillslot);
		}

		foreach (GameObject slotObject in _skillSelectedSlotList)
		{
			if (slotObject == null)
				continue;
			
			UIUseSkillSlot useSkillslot = slotObject.GetComponentInChildren<UIUseSkillSlot>();
			if (useSkillslot == null)
				continue;
			
			_uiUseSkillSlotList.Add(useSkillslot);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateHaveSkillSlotList(List<BattleSkill> haveSkillList)
	{
		if (haveSkillList == null)
			return;

		for (int i = 0; i < _uiHaveSkillSlotList.Count; ++i)
		{
			UIHaveSkillSlot uiHaveSkillSlot = _uiHaveSkillSlotList[i];
			if (uiHaveSkillSlot == null)
				continue;

			if (i >= haveSkillList.Count)
			{
				uiHaveSkillSlot.gameObject.SetActive(false);
				continue;
			}

			BattleSkill skill = haveSkillList[i];
			if (skill == null)
			{
				uiHaveSkillSlot.gameObject.SetActive(false);
				continue;
			}
			
			if (skill._skillLevel <= 0)
			{
				uiHaveSkillSlot.gameObject.SetActive(false);
				continue;
			}
			
			uiHaveSkillSlot._battleSkill = skill;
			uiHaveSkillSlot.SkillIconUpdate();
		}
	}

	public void SelectSlot(UIHaveSkillSlot skillslot)
	{
		DeactivateSelectedMark();

		if (skillslot == null)
			return;

		_selectedHaveSkillSlot = skillslot;
		_selectedHaveSkillSlot._selectFrame.SetActive(true);

		SceneBattleSkillSelect.GetInstance().UpdateSkillDetails(skillslot._battleSkill);
	}

	public void DeactivateSelectedMark()
	{
		foreach (UIHaveSkillSlot haveSkillslot in _uiHaveSkillSlotList)
		{
			haveSkillslot._selectFrame.SetActive(false);
		}
	}

	public void ResetUseSkillList()
	{
		foreach (UIUseSkillSlot useSkillSlot in _uiUseSkillSlotList)
		{
			useSkillSlot.ResetIcon();
		}
	}
}
