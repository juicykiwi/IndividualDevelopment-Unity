using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneBattleSkillSelect : MonoBehaviour {

	public static SceneBattleSkillSelect GetInstance() { return _instance; }
	protected static SceneBattleSkillSelect _instance = null;

	protected SkillSlotController _skillSlotController = null;

	protected UILabel _labelSkillLevel = null;
	protected UILabel _labelSkillDamage = null;
	protected UILabel _labelSkillHeal = null;
	protected UILabel _labelEnchantPoint = null;

	public string _nextSceneName = "";

	void Awake () {
		if (_instance == null)
		{
			_instance = this;
		}

		UILabel[] labelList = (UILabel[])FindObjectsOfType<UILabel>();
		foreach (UILabel label in labelList)
		{
			switch (label.name)
			{
			case "EPLabel":
				_labelEnchantPoint = label;
				break;

			case "SkillLevelLabel":
				_labelSkillLevel = label;
				break;

			case "DamageLabel":
				_labelSkillDamage = label;
				break;

			case "HealLabel":
				_labelSkillHeal = label;
				break;

			default:
				break;
			}
		}
	}

	// Use this for initialization
	void Start () {
		_skillSlotController = FindObjectOfType<SkillSlotController>();
		if (_skillSlotController == null)
			return;

		SkillManager.GetInstance().UpdateHaveSkillList();
		UpdateHaveSkillSlotList();

		UpdateSkillEnchantPoint(SkillManager.GetInstance()._skillEnchantPoint);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected void UpdateHaveSkillSlotList()
	{
		List<BattleSkill> haveSkilLlist = SkillManager.GetInstance()._haveSkilLlist;
		if (haveSkilLlist == null)
			return;

		_skillSlotController.UpdateHaveSkillSlotList(haveSkilLlist);
	}

	public void UpdateSkillDetails(BattleSkill skill)
	{
		if (skill == null)
		{
			_labelSkillLevel.text = string.Format("{0}", 0);
			_labelSkillDamage.text = string.Format("{0}", 0);
			_labelSkillHeal.text = string.Format("{0}", 0);
			return;
		}

		_labelSkillLevel.text = skill._skillLevel.ToString();

		SkillLevelData skillLevelData = SkillManager.GetInstance ()._skillLevelData;
		if (skillLevelData)
		{
			_labelSkillDamage.text = string.Format("+{0}x", skillLevelData._skillLevelValueList[skill._skillLevel-1]);
			_labelSkillHeal.text = string.Format("+{0}x", skillLevelData._skillLevelValueList[skill._skillLevel-1]);
		}
	}

	public void UpdateSkillEnchantPoint(int enchantPoint)
	{
		_labelEnchantPoint.text = enchantPoint.ToString();
	}

	public void DoSkillEnchant()
	{
		if (SkillManager.GetInstance()._skillEnchantPoint <= 0)
			return;

		BattleSkill skill = _skillSlotController._selectedHaveSkillSlot._battleSkill;
		if (skill == null)
			return;

		if(skill._skillLevel >= SkillManager.GetInstance()._skillLevelData._skillLevelValueList.Count)
			return;

		skill._skillLevel++;
		SkillManager.GetInstance()._skillEnchantPoint--;

		_skillSlotController.ResetUseSkillList();
		UpdateSkillDetails(skill);
		UpdateSkillEnchantPoint(SkillManager.GetInstance()._skillEnchantPoint);
	}

	public void DoNextScene()
	{
		if (SkillManager.GetInstance() == null)
			return;

		List<BattleSkill> useSkillList = new List<BattleSkill>();

		foreach (UIUseSkillSlot skillSlot in _skillSlotController._uiUseSkillSlotList)
		{
			if (skillSlot == null)
				continue;

			BattleSkill skill = skillSlot._battleSkill;
			if (skill == null)
				continue;

			useSkillList.Add(skill);
		}

		SkillManager.GetInstance().UpdateUseSkillList(useSkillList);
		Application.LoadLevel(_nextSceneName);
	}
}
