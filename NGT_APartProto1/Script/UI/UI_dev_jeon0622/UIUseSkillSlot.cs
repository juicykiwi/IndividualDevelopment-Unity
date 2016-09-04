using UnityEngine;
using System.Collections;

public class UIUseSkillSlot : MonoBehaviour {

	public BattleSkill _battleSkill = null;

	protected UIButton _uiButton = null;
	protected SkillSlotController _skillSlotController = null;

	protected string _skillIconName = "";
	public GameObject _selectFrame = null;

	void Awake()
	{
		_uiButton = this.GetComponent<UIButton>();
		
		_skillSlotController = this.GetComponentInParent<SkillSlotController>();

		if (_battleSkill == null)
		{
			_skillIconName = "F_profile_02";
			_uiButton.normalSprite = _skillIconName;
			return;
		}
		
		if (_battleSkill)
		{
			_skillIconName = _battleSkill._skillIconName;
			_uiButton.normalSprite = _skillIconName;
		}
		else
		{
			_skillIconName = "F_profile_02";
			_uiButton.normalSprite = _skillIconName;
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPress()
	{
	}
	
	void OnClick()
	{
		do
		{
			if (_skillSlotController._selectedHaveSkillSlot == null)
				break;

			UIHaveSkillSlot skillSlot = _skillSlotController._selectedHaveSkillSlot.GetComponentInChildren<UIHaveSkillSlot>();
			if (skillSlot == null)
				break;

			if (skillSlot._battleSkill == null)
				break;

			_battleSkill = skillSlot._battleSkill;
			if (_battleSkill)
			{
				_skillIconName = _battleSkill._skillIconName;
				_uiButton.normalSprite = _skillIconName;
			}
			else
			{
				_skillIconName = "F_profile_02";
				_uiButton.normalSprite = _skillIconName;
			}
		} while (false);
	}
	
	void OnHover()
	{
	}
	
	void OnMouseOver()
	{
	}

	public void ResetIcon()
	{
		_battleSkill = null;
		_skillIconName = "F_profile_02";
		_uiButton.normalSprite = _skillIconName;
	}
}
