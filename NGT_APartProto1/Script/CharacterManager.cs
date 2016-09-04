using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour {

	static protected CharacterManager _instance = null;
	static public CharacterManager GetInstance() { return _instance; }

	public List<BaseCharacter> _characterList = new List<BaseCharacter>();
	public BaseCharacter _playerSummoner = null;

	public Dictionary<int, Vector3> _formationPosDict = new Dictionary<int, Vector3>();
	
	void Awake () {
		if (_instance == null)
		{
			_instance = this;
		}

		BaseCharacter[] baseCharacterList = FindObjectsOfType<BaseCharacter>();

		for (int i = 0; i < baseCharacterList.Length; ++i)
		{
			_characterList.Add(baseCharacterList[i]);
			if (baseCharacterList[i]._mainType == CharacterMainType.Summoner)
			{
				_playerSummoner = baseCharacterList[i];
			}
		}

		UpdateFormationPosList();

		Debug.Log(string.Format("init BaseCharacter count : {0}", _characterList.Count));
	}

	// Use this for initialization
	void Start () {
		SetPlayerSummonerSkill();
	}
	
	// Update is called once per frameB
	void Update () {
	
	}

	void SetPlayerSummonerSkill()
	{
		do
		{
			if (SkillManager.GetInstance() == null)
				break;
			
			List<BattleSkill> useSkillList = SkillManager.GetInstance()._useSkillList;
			if (useSkillList == null)
				break;
			
			if (_playerSummoner == null)
				break;
			
			if (_playerSummoner.GetSkillController() == null)
				break;

			if (_playerSummoner.GetSkillController()._skillList == null)
				break;

			_playerSummoner.GetSkillController().ClearSkillListOtherBaseAttack();
			for (int i = 0; i < useSkillList.Count; ++i)
			{
				BattleSkill skill = useSkillList[i];
				if (skill == null)
					continue;

				skill._skillSlotType = (i + SkillSlotType.Skill01);
				_playerSummoner.GetSkillController()._skillList.Add(skill);
			}
			
		} while(false);
	}

	public List<BaseCharacter> EqualBattleSideCharacters(BattleSide battleSide)
	{
		List<BaseCharacter> equalBattleSideCharacters = new List<BaseCharacter>();

		foreach (BaseCharacter character in _characterList)
		{
			if (character == null)
				continue;

			if (character._battleSide != battleSide)
				continue;

			equalBattleSideCharacters.Add(character);
		}

		return equalBattleSideCharacters;
	}

	public BaseCharacter LowHpCharacter(BattleSide battleSide)
	{
		List<BaseCharacter> characters = EqualBattleSideCharacters(battleSide);

		BaseCharacter lowHpCharacter = null;

		foreach (BaseCharacter character in characters)
		{
			if (character.IsAlive() == false)
				continue;

			if (lowHpCharacter == null)
			{
				lowHpCharacter = character;
				continue;
			}

			if (character.getStat()._hpCurrent < lowHpCharacter.getStat()._hpCurrent)
			{
				lowHpCharacter = character;
			}
		}

		return lowHpCharacter;
	}

	public BaseCharacter LowHpRatioCharacter(BattleSide battleSide)
	{
		List<BaseCharacter> characters = EqualBattleSideCharacters(battleSide);
		
		BaseCharacter lowHpCharacter = null;
		
		foreach (BaseCharacter character in characters)
		{
			if (character.IsAlive() == false)
				continue;
			
			if (lowHpCharacter == null)
			{
				lowHpCharacter = character;
				continue;
			}

			if (character.getStat().GetHpCurrentRatio() < lowHpCharacter.getStat().GetHpCurrentRatio())
			{
				lowHpCharacter = character;
			}
		}
		
		return lowHpCharacter;
	}

	public void UpdateFormationPosList()
	{
		if (_playerSummoner == null)
			return;

		List<BaseCharacter> characterList = EqualBattleSideCharacters(BattleSide.A);
		if (characterList.Count <= 0)
			return;

		_formationPosDict.Clear();
		foreach (BaseCharacter character in characterList)
		{
			if (character == null)
				continue;

			Vector3 pos = character.transform.position - _playerSummoner.transform.position;
			_formationPosDict.Add(character._unitIndex, pos);

//			Debug.Log(string.Format("formationPosDict.Add pos : {0} / {1} / {2}", pos.x, pos.y, pos.z));
		}
	}

	public void ChangeFormationForCameraCenter(float formationAngle)
	{
		Quaternion quatAngle = Quaternion.AngleAxis(formationAngle, Vector3.up);
		Vector3 direction = quatAngle * Vector3.forward;
//		Debug.Log(string.Format("direction 2 : {0} / {1} / {2}", direction.x, direction.y, direction.z));
		
		Vector3 screenCenterPos = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
//		Debug.Log(string.Format("screen center pos : {0} / {1} / {2}", screenCenterPos.x, screenCenterPos.y, screenCenterPos.z));
		
		Ray ray = Camera.main.ScreenPointToRay(screenCenterPos);
		RaycastHit[] hits = Physics.RaycastAll(ray, 1000.0f);
		if (hits.Length <= 0)
			return;
		
		bool isFieldHit = false;
		Vector3 fieldHitPos = new Vector3();
		
		foreach (RaycastHit hit in hits)
		{
			if (hit.collider.tag != "Tag_Field")
				continue;
			
			isFieldHit = true;
			fieldHitPos = hit.point;
//			Debug.Log(string.Format("fieldHitPos : {0} / {1} / {2}", fieldHitPos.x, fieldHitPos.y, fieldHitPos.z));
			break;
		}
		
		if (isFieldHit == false)
			return;

		List<BaseCharacter> allyCharacterList =  EqualBattleSideCharacters(BattleSide.A);
		if (allyCharacterList == null)
			return;

		foreach (BaseCharacter character in allyCharacterList)
		{
			if (character == null)
				continue;
			
			Vector3 pos = _formationPosDict[character._unitIndex];
//			Debug.Log(string.Format("_formationPosDict pos : {0} / {1} / {2}", pos.x, pos.y, pos.z));
			
			Vector3 destPos = fieldHitPos + (quatAngle * pos);
			
			character.SetForceMoveDest(destPos);
//			Debug.Log(string.Format("destPos pos : {0} / {1} / {2}", destPos.x, destPos.y, destPos.z));
		}
	}

	public void ChangeFormationForCharPosAverage(float formationAngle)
	{
		Quaternion quatAngle = Quaternion.AngleAxis(formationAngle, Vector3.up);
		Vector3 direction = quatAngle * Vector3.forward;
//		Debug.Log(string.Format("direction 2 : {0} / {1} / {2}", direction.x, direction.y, direction.z));
		
		List<BaseCharacter> allyCharacterList =  EqualBattleSideCharacters(BattleSide.A);
		if (allyCharacterList == null)
			return;

		if (allyCharacterList.Count <= 0)
			return;

		Vector3 allyTotalPos = new Vector3();
		Vector3 allyAveragePos = new Vector3();

		foreach (BaseCharacter character in allyCharacterList)
		{
			allyTotalPos += character.transform.position;
		}

		allyAveragePos = allyTotalPos / allyCharacterList.Count;

		
		foreach (BaseCharacter character in allyCharacterList)
		{
			if (character == null)
				continue;
			
			Vector3 pos = _formationPosDict[character._unitIndex];
//			Debug.Log(string.Format("_formationPosDict pos : {0} / {1} / {2}", pos.x, pos.y, pos.z));
			
			Vector3 destPos = allyAveragePos + (quatAngle * pos);
			
			character.SetForceMoveDest(destPos);
//			Debug.Log(string.Format("destPos pos : {0} / {1} / {2}", destPos.x, destPos.y, destPos.z));
		}
	}

	public void ChangeFormationForSummonerPos(float formationAngle)
	{
		Quaternion quatAngle = Quaternion.AngleAxis(formationAngle, Vector3.up);
		Vector3 direction = quatAngle * Vector3.forward;
		//		Debug.Log(string.Format("direction 2 : {0} / {1} / {2}", direction.x, direction.y, direction.z));
		
		List<BaseCharacter> allyCharacterList =  EqualBattleSideCharacters(BattleSide.A);
		if (allyCharacterList == null)
			return;
		
		if (allyCharacterList.Count <= 0)
			return;

		if (_playerSummoner == null)
			return;
		
		foreach (BaseCharacter character in allyCharacterList)
		{
			if (character == null)
				continue;
			
			Vector3 pos = _formationPosDict[character._unitIndex];
			//			Debug.Log(string.Format("_formationPosDict pos : {0} / {1} / {2}", pos.x, pos.y, pos.z));
			
			Vector3 destPos = _playerSummoner.transform.position + (quatAngle * pos);
			
			character.SetForceMoveDest(destPos);
			//			Debug.Log(string.Format("destPos pos : {0} / {1} / {2}", destPos.x, destPos.y, destPos.z));
		}
	}
}
