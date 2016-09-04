using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIButtonSkill : MonoBehaviour {

	public SkillSlotType _skillSlotType = SkillSlotType.None;
	public CameraConstroll _shakeCamera = null;

	CharacterManager _characterManager = null;

	protected UIButton _uiButton = null;

	void Awake() {
		_characterManager = (CharacterManager)FindObjectOfType<CharacterManager>();

		_uiButton = this.GetComponent<UIButton>();


	}

	// Use this for initialization
	void Start () {
		_shakeCamera = (CameraConstroll)this.GetComponentInChildren<CameraConstroll>();

		SkillIconUpdate();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SkillIconUpdate()
	{
		if (SkillManager.GetInstance() == null)
			return;

		if (_skillSlotType < SkillSlotType.Skill01)
			return;

		int useSkillSlotIndex = _skillSlotType - SkillSlotType.Skill01;

		List<BattleSkill> useSkillList = SkillManager.GetInstance()._useSkillList;
		if (useSkillList == null)
			return;

		if (useSkillSlotIndex >= useSkillList.Count)
			return;

		BattleSkill skill = useSkillList[useSkillSlotIndex];
		if (skill == null)
			return;

		if (_uiButton == null)
			return;

		_uiButton.normalSprite = skill._skillIconName;
	}

	void OnPress()
	{
	}

	void OnClick()
	{
		if (_characterManager._playerSummoner)
		{
			_characterManager._playerSummoner.OnPlayerSkill(_skillSlotType);
		}

		if (this._skillSlotType == SkillSlotType.Skill03) {
			if (_shakeCamera != null) {
				_shakeCamera.StartCoroutine ("HeroFocus");

				_shakeCamera.StartCoroutine("ZoomCamera");
				//_shakeCamera.StartCoroutine("ShakeCamera");
				_shakeCamera.StartCoroutine("ShadowScreen");
				}
			}
	}

	void OnHover()
	{
	}

	void OnMouseOver()
	{
	}
}
