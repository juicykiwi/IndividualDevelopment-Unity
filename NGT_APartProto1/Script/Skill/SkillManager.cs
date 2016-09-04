using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour {

	protected static SkillManager _instance = null;
	public static SkillManager GetInstance() { return _instance; }

	public List<GameObject> _summonerSkillPrefabList = new List<GameObject>();

	public List<BattleSkill> _allSkillList = new List<BattleSkill>();
	public List<BattleSkill> _haveSkilLlist = new List<BattleSkill>();
	public List<BattleSkill> _useSkillList = new List<BattleSkill>();

	public SkillLevelData _skillLevelData = null;

	public int _skillEnchantPoint = 10;

	void Awake()
	{
		DontDestroyOnLoad(this);

		if (_instance == null)
			_instance = this;

		LoadBattleSkillWithPrefab();

		_skillLevelData = this.GetComponent<SkillLevelData>();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	protected void LoadBattleSkillWithPrefab()
	{
		foreach (GameObject skillPrefab in _summonerSkillPrefabList)
		{
			if (skillPrefab == null)
				continue;
			
			BattleSkill skill = skillPrefab.GetComponent<BattleSkill>();
			if (skill == null)
				continue;

			skill._skillLevel = 1;
			
			_allSkillList.Add(skill);
		}
	}

	public void UpdateHaveSkillList()
	{
		_haveSkilLlist.Clear();

		foreach (BattleSkill skill in _allSkillList)
		{
			if (skill._skillLevel > 0)
			{
				_haveSkilLlist.Add(skill);
			}
		}
	}

	public void UpdateUseSkillList(List<BattleSkill> useSkillList)
	{
		_useSkillList.Clear();

		foreach (BattleSkill skill in useSkillList)
		{
			if (skill == null)
				continue;

			BattleSkill useSkill = (BattleSkill)Instantiate(skill);
			DontDestroyOnLoad(useSkill);
			_useSkillList.Add(useSkill);
		}
	}
}






