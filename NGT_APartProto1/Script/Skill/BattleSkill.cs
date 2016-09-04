using UnityEngine;
using System.Collections;

public class BattleSkill : MonoBehaviour {

	public int _skillLevel = 1;
	public int _weight = 1;
	public float _range = 1.0f;

	public float _castTime = 0.0f;
	public float _coolTime = 0.0f;

	protected float _remainCoolTime = 0.0f;
	public float GetRemainCoolTime() { return _remainCoolTime; }

	public GameObject _castingEffect = null;
	public float _castingEffectDestroyTime = 0.0f;

	protected BattleAction[] _battleActions;
	public BattleAction[] GetBattleActions() { return _battleActions; }

	protected BattleAction[] _battleDamageOnTimeActions;
	public BattleAction[] GetBattleDamageOnTimeActions() { return _battleDamageOnTimeActions; }
	public SkillSlotType _skillSlotType = SkillSlotType.None;
	public SkillMotionType _skillMotionType = SkillMotionType.None;

	public string _skillIconName = "";

	void Awake()
	{
	}

	// Use this for initialization
	void Start () {
		_battleActions = (BattleAction[])this.GetComponentsInChildren<BattleAction>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_remainCoolTime > 0)
			_remainCoolTime -= Time.deltaTime;
	}

	public void ResetCoolTime()
	{ 
		float coolTimepercent = Random.Range(0.8f, 1.2f);
		_remainCoolTime = _coolTime * coolTimepercent;
	}
}
