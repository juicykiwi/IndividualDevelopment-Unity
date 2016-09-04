using UnityEngine;
using System.Collections;

public class UIHaveSkillSlot : MonoBehaviour {

	public BattleSkill _battleSkill = null;

//	protected bool _isClicked = false;
//	public bool isClicked { get { return _isClicked; } set { _isClicked = value; } }

	protected UIButton _uiButton = null;

	protected string _skillIconName = "";
	public GameObject _selectFrame = null;

	protected SkillSlotController _skillSlotController = null;

	void Awake()
	{
		_uiButton = this.GetComponent<UIButton>();

		_skillSlotController = this.GetComponentInParent<SkillSlotController>();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SkillIconUpdate()
	{
		if (_battleSkill == null)
			return;

		_skillIconName = _battleSkill._skillIconName;
		_uiButton.normalSprite = _skillIconName;
	}

	void OnPress()
	{
	}
	
	void OnClick()
	{
		_skillSlotController.SelectSlot(this);
	}
	
	void OnHover()
	{
	}
	
	void OnMouseOver()
	{
	}
}
