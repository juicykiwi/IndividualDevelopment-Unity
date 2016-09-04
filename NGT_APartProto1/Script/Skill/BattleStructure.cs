using UnityEngine;
using System.Collections;

public enum ActionMainType
{
	None,
	Melee,
	UseCollider,
	Range,
	Heal,
	UseColliderForHeal,
};

public enum SkillDamageOnTiming
{
	AnimationEventTrigger,
	UseDamageOnTime,
};

public enum SkillRangeSubType
{
	Normal,
	Pierce,
};

public enum ActionRangeSubType
{
	Normal,
	Pierce,
};

public enum SkillSlotType
{
	None		= 0,
	BaseAttack	= 1,
	Skill01		= 2,
	Skill02		= 3,
	Skill03		= 4,
};

public enum SkillMotionType
{
	None,
	Motion01,
	Motion02,
	Motion03,
	Motion04,
};

public enum ActionHitType
{
	Normal,
	LightHit,
	MiddleHit,
	Stun,
	Knockback,
	Knockdown,
	Airborne,
};

public class BattleStructure : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
