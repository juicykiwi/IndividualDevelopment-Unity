using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillController : MonoBehaviour {

	public float _skillEnableTime = 0.0f;
	protected bool _skillEnabled = false;

	public GameObject[] _skills;
	public List<BattleSkill> _skillList = new List<BattleSkill>();
	protected int _skillWeightTotal = 0;

	void Awake () {
		for (int i = 0; i < _skills.Length; ++i)
		{
			if (_skills[i] == null)
				break;
			
			GameObject skillObject = (GameObject)Instantiate(_skills[i]);
			if (skillObject == null)
				break;
			
			BattleSkill skill = (BattleSkill)skillObject.GetComponent<BattleSkill>();
			if (skill == null)
				break;

			skill._skillSlotType = (SkillSlotType)(i + 1);
			
			_skillList.Add(skill);
			_skillWeightTotal += skill._weight;
		}

		float skillEnableTimePercent = Random.Range(0.8f, 1.2f);
		_skillEnableTime = _skillEnableTime * skillEnableTimePercent;
		if (_skillEnableTime <= 0.0f)
			_skillEnabled = true;
	}

	// Use this for initialization
	void Start () {
	
	}

	public void init()
	{

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void ClearSkillListOtherBaseAttack()
	{
		List<BattleSkill> removeSkillList = new List<BattleSkill>();

		foreach (BattleSkill skill in _skillList)
		{
			if (skill == null)
				continue;

			if (skill._skillSlotType == SkillSlotType.BaseAttack)
				continue;

			removeSkillList.Add(skill);
		}

		foreach (BattleSkill skill in removeSkillList)
		{
			_skillList.Remove(skill);
		}
	}

//	public BattleSkill GetUseRandomSkill()
//	{
//		if (_skillEnabled == false)
//		{
//			if (IsInvoking("SkillEnableTrigger") == false)
//				Invoke("SkillEnableTrigger", _skillEnableTime);
//
//			return _baseSkill;
//		}
//
//		int totalWeight = 0;
//		int randValue = Random.Range(0, _skillWeightTotal);
//
//		for (int i = 0; i < _skillList.Count; ++i)
//		{
//			BattleSkill skill = (BattleSkill)_skillList[i];
//			if (skill == null)
//				continue;
//
//			totalWeight += skill._weight;
//			if (randValue < totalWeight)
//				return skill;
//
//			if (i == _skillList.Count -1)
//				return skill;
//		}
//
//		return null;
//	}

	public void SetSkillSlot(int index, BattleSkill skill)
	{
		if (index < _skillList.Count)
			_skillList.RemoveAt(index);

		_skillList.Insert(index, skill);
	}

	public BattleSkill GetUseSkillWithSlotIndex(SkillSlotType slotType)
	{
		if (_skillEnabled == false)
		{
			if (IsInvoking("SkillEnableTrigger") == false)
				Invoke("SkillEnableTrigger", _skillEnableTime);
		}

		BattleSkill useSkill = null;

		foreach (BattleSkill skill in _skillList)
		{
			if (skill == null)
				continue;

			if (skill._skillSlotType != slotType)
				continue;

			if (skill.GetRemainCoolTime() > 0.0f)
				continue;

			if (skill._skillSlotType >= SkillSlotType.Skill01 &&
			    _skillEnabled == false)
			{
				continue;
			}

			useSkill = skill;
		}

		return useSkill;
	}

//	public BattleSkill GetUseSkillOnCooltime()
//	{
//		if (_skillEnabled == false)
//		{
//			if (IsInvoking("SkillEnableTrigger") == false)
//				Invoke("SkillEnableTrigger", _skillEnableTime);
//			
//			return _baseSkill;
//		}
//
//		BattleSkill baseSkill = null;
//		BattleSkill useSkill = null;
//
//		for (int i = 0; i < _skillList.Count; ++i)
//		{
//			BattleSkill skill = (BattleSkill)_skillList[i];
//			if (skill == null)
//				continue;
//
//			if (skill._skillSlotType == SkillSlotType.BaseAttack)
//			{
//				baseSkill = skill;
//				continue;
//			}
//
//			if (skill.GetRemainCoolTime() > 0)
//				continue;
//
//			useSkill = skill;
//		}
//
//		return useSkill ? useSkill : baseSkill;
//	}

	public void NowEnableSkills(out BattleSkill baseAttack, ref List<BattleSkill> skills, out int totalWeight)
	{
		totalWeight = 0;
		baseAttack = null;

		foreach (BattleSkill skill in _skillList)
		{
			if (skill == null)
				continue;

			if (skill._skillSlotType == SkillSlotType.None)
				continue;

			if (skill.GetRemainCoolTime() > 0)
				continue;

			if (skill._skillSlotType == SkillSlotType.BaseAttack)
			{
				baseAttack = skill;
				continue;
			}

			if (skill._skillSlotType >= SkillSlotType.Skill01 &&
			    _skillEnabled == false)
			{
				continue;
			}

			skills.Add(skill);
			totalWeight += skill._weight;
		}
	}

	public BattleSkill GetUseCreatureSkill()
	{
		if (_skillEnabled == false)
		{
			if (IsInvoking("SkillEnableTrigger") == false)
				Invoke("SkillEnableTrigger", _skillEnableTime);
		}

		int totlaWeight = 0;
		BattleSkill useSkill = null;
		BattleSkill baseSkill = null;
		List<BattleSkill> enableSkills = new List<BattleSkill>();

		NowEnableSkills(out baseSkill, ref enableSkills, out totlaWeight);
		if (enableSkills.Count <= 0)
			return baseSkill;

		int currectWeight =  Random.Range(0, totlaWeight);

		foreach (BattleSkill skill in enableSkills)
		{
			if (currectWeight <= skill._weight)
			{
				useSkill = skill;
				break;
			}

			currectWeight -= skill._weight;
		}

		return (useSkill == null) ? baseSkill : useSkill;
	}

	protected void SkillEnableTrigger()
	{
		_skillEnabled = true;
	}
}
