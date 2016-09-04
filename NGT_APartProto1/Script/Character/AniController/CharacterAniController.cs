using UnityEngine;
using System.Collections;

public enum CharacterAniState
{
	None,
	Idle,
	Walk,
	Cast,
	Attack,
	Skill01,
	Skill02,
	Skill03,
	LightHit,
	MiddleHit,
	Stun,
	Knockback,
	KnockdownStart,
	KnockdownWait,
	KnockdownEnd,
	AirborneStart,
}

public class CharacterAniController : MonoBehaviour {

	protected Animator _animator;

	void Awake()
	{
		_animator = GetComponentInChildren<Animator>();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ResetAllAniTrigger()
	{
		_animator.SetBool(string.Format("{0}", CharacterAniState.Idle.ToString()), false);
		_animator.SetBool(string.Format("{0}", CharacterAniState.Walk.ToString()), false);
		_animator.SetBool(string.Format("{0}", CharacterAniState.Cast.ToString()), false);
		_animator.SetBool(string.Format("{0}", CharacterAniState.Attack.ToString()), false);
		_animator.SetBool(string.Format("{0}", CharacterAniState.Skill01.ToString()), false);
		_animator.SetBool(string.Format("{0}", CharacterAniState.Skill02.ToString()), false);
		_animator.SetBool(string.Format("{0}", CharacterAniState.Skill03.ToString()), false);
		_animator.SetBool(string.Format("{0}", CharacterAniState.LightHit.ToString()), false);
		_animator.SetBool(string.Format("{0}", CharacterAniState.MiddleHit.ToString()), false);
		_animator.SetBool(string.Format("{0}", CharacterAniState.Stun.ToString()), false);
		_animator.SetBool(string.Format("{0}", CharacterAniState.Knockback.ToString()), false);
		_animator.SetBool(string.Format("{0}", CharacterAniState.KnockdownStart.ToString()), false);
		_animator.SetBool(string.Format("{0}", CharacterAniState.KnockdownWait.ToString()), false);
		_animator.SetBool(string.Format("{0}", CharacterAniState.KnockdownEnd.ToString()), false);
		_animator.SetBool(string.Format("{0}", CharacterAniState.AirborneStart.ToString()), false);
	}

	public void SetAnimationWithAniState(CharacterAniState aniState)
	{
		ResetAllAniTrigger();

		_animator.SetBool(string.Format("{0}", aniState.ToString()), true);
	}

	public void SetAnimation(BaseAI.AIState aiState)
	{
		ResetAllAniTrigger();

		switch (aiState)
		{
		case BaseAI.AIState.Idle:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.Idle.ToString()), true);
		}
			break;

		case BaseAI.AIState.Move:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.Walk.ToString()), true);
		}
			break;

		case BaseAI.AIState.MoveCommand:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.Walk.ToString()), true);
		}
			break;

		case BaseAI.AIState.SkillSelect:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.Idle.ToString()), true);
		}
			break;

		case BaseAI.AIState.PreAttack:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.Cast.ToString()), true);
		}
			break;

		case BaseAI.AIState.Attack:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.Attack.ToString()), true);
		}
			break;

		case BaseAI.AIState.SkillAttack:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.Skill01.ToString()), true);
		}
			break;

		case BaseAI.AIState.PostAttack:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.Idle.ToString()), true);
		}
			break;

		case BaseAI.AIState.LightHit:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.LightHit.ToString()), true);
		}
			break;

		case BaseAI.AIState.MiddleHit:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.MiddleHit.ToString()), true);
		}
			break;

		case BaseAI.AIState.Stun:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.Stun.ToString()), true);
		}
			break;

		case BaseAI.AIState.Knockback:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.Knockback.ToString()), true);
		}
			break;

		case BaseAI.AIState.KnockdownStart:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.KnockdownStart.ToString()), true);
		}
			break;

		case BaseAI.AIState.KnockdownWait:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.KnockdownWait.ToString()), true);
		}
			break;

		case BaseAI.AIState.KnockdownEnd:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.KnockdownEnd.ToString()), true);
		}
			break;

		case BaseAI.AIState.AirborneStart:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.AirborneStart.ToString()), true);
		}
			break;

		case BaseAI.AIState.Die:
		{
			_animator.SetBool(string.Format("{0}", CharacterAniState.Idle.ToString()), true);
		}
			break;

		default:
			_animator.SetBool(string.Format("{0}", CharacterAniState.Idle.ToString()), true);
			break;
		}
	}
}
