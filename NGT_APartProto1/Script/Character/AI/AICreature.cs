using UnityEngine;
using System.Collections;

public class AICreature : BaseAI {



	protected override void Awake () {
		base.Awake ();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (_baseCharacter.getStat()._hpCurrent <= 0)
		{
			// state change to Dead
			setAIState(AIState.Die);
			return;
		}

		UpdateAI();
	}

	void UpdateAI()
	{
		switch (_aiState)
		{
		case AIState.Idle:
		{
			if ((idleTimeBeforeBattleStart -= Time.deltaTime) > 0.0f)
			{
				break;
			}
			
			_baseCharacter.SetTargetCharacter(FindTarget());
			
			if (_baseCharacter.GetTargetCharacter() == null)
			{
				_destPos = _transform.position;
				break;
			}

			_baseCharacter.UpdateLookAt();

			if (_isAttackDelay == true)
				break;
			
			_destPos = _baseCharacter.GetTargetCharacter().transform.position;
			
			float distance = Vector3.Distance(_destPos, _transform.position);
			if (distance > _baseCharacter.getAttackRange())
			{
				setAIState(AIState.Move);
				break;
			}

			setAIState(AIState.SkillSelect);
		}
			break;
			
		case AIState.Move:
		{
			_baseCharacter.SetTargetCharacter(FindTarget());
			if (_baseCharacter.GetTargetCharacter() == null)
			{
				setAIState(AIState.Idle);
				break;
			}
			
			float distance = _baseCharacter.DistanceTarget();
			if (distance <= _baseCharacter.getAttackRange())
			{
				if (_isAttackDelay == true)
				{
					setAIState(AIState.Idle);
					break;
				}

				setAIState(AIState.SkillSelect);
				break;
			}

			_baseCharacter.MoveToTarget();
		}
			break;

		case AIState.MoveCommand:
		{
			float distance = Vector3.Distance(_transform.position, _baseCharacter._forceMoveDest);
			if (distance < 1.0f)
			{
				setAIState(AIState.Idle);
			}

			_baseCharacter.MoveToForcePosDest();
		}
			break;
			
		case AIState.SkillSelect:
		{
			BattleSkill useSkill = _baseCharacter.GetSkillController().GetUseCreatureSkill();
			if (useSkill == null)
			{
				setAIState(AIState.Idle);
				break;
			}
			
			_baseCharacter.SetUseSkill(useSkill);
			
			switch (useSkill._skillSlotType)
			{
			case SkillSlotType.BaseAttack:
			{
				setAIState(AIState.Attack);
				useSkill.ResetCoolTime();
			}
				break;
				
			case SkillSlotType.Skill01:
			case SkillSlotType.Skill02:
			case SkillSlotType.Skill03:
			{
				if (useSkill._castTime < 0.5f)
				{
					setAIState(AIState.SkillAttack);
					useSkill.ResetCoolTime();
					break;
				}
				
				setAIState(AIState.PreAttack);
			}
				break;
				
			default:
				setAIState(AIState.Idle);
				break;
			}
		}
			break;
			
		case AIState.PreAttack:
		{
			if (_baseCharacter.GetUseSkill() == null)
			{
				setAIState(AIState.Idle);
				break;
			}
			
			if (IsInvoking("SkillCastComplete") == true)
				break;
			
			Invoke("SkillCastComplete", _baseCharacter.GetUseSkill()._castTime);
		}
			break;
			
		case AIState.Attack:
		{
		}
			break;
			
		case AIState.SkillAttack:
		{
		}
			break;
			
		case AIState.PostAttack:
		{
			setAIState(AIState.Idle);
		}
			break;

		case AIState.LightHit:
		case AIState.MiddleHit:
		case AIState.Stun:
		case AIState.Knockback:
		case AIState.KnockdownStart:
		case AIState.KnockdownWait:
		case AIState.KnockdownEnd:
		case AIState.AirborneStart:
		{
		}
			break;
			
		case AIState.Die:
		{
		}
			break;
			
		default:
		{
		}
			break;
		}
	}
	
	public override void setAIState(AIState aiState)
	{
		CharacterAniState aniState = CharacterAniState.None;

		switch (aiState)
		{

		case AIState.PreAttack:
		{
			_baseCharacter.GetUseSkill().ResetCoolTime();
			_baseCharacter.ActiveCastingEffect();
		}
			break;

		case AIState.Attack:
		case AIState.SkillAttack:
		{
			if (_baseCharacter.GetUseSkill() == null)
				return;

			switch (_baseCharacter.GetUseSkill()._skillMotionType)
			{
			case SkillMotionType.Motion01:
				aniState = CharacterAniState.Attack;
				break;

			case SkillMotionType.Motion02:
				aniState = CharacterAniState.Skill01;
				break;

			case SkillMotionType.Motion03:
				aniState = CharacterAniState.Skill02;
				break;

			case SkillMotionType.Motion04:
				aniState = CharacterAniState.Skill03;
				break;
			}
		}
			break;

		case AIState.PostAttack:
		{
			ActiveAttackDelay();
		}
			break;

		case AIState.LightHit:
		case AIState.MiddleHit:
		case AIState.Stun:
		case AIState.Knockback:
		case AIState.KnockdownStart:
		case AIState.KnockdownWait:
		case AIState.KnockdownEnd:
		case AIState.AirborneStart:
		{
			SkillCastFail();
			_baseCharacter.DeactivateCastingEffect();
			
			switch (_aiState)
			{
			case AIState.PreAttack:
			case AIState.Attack:
			{
				ActiveAttackDelay();
			}
				break;
			}
		}
			break;

		case AIState.Die:
		{
			SkillCastFail();

			if (_baseCharacter._battleSide == BattleSide.A) {
				Debug.Log ("ally creature die");
			}
			else if (_baseCharacter._battleSide == BattleSide.B) {
				Debug.Log ("enemy creature die");
			}

			_characterManager._characterList.Remove(_baseCharacter);
			Destroy(this.gameObject);
		}
			break;

		default:
		{
		}
			break;
		}

		_aiState = aiState;

		if (aniState == CharacterAniState.None)
		{
			_aniController.SetAnimation(_aiState);
		}
		else
		{
			_aniController.SetAnimationWithAniState(aniState);
		}
	}

	public BaseCharacter FindTarget()
	{
		BaseCharacter targetCharacter = null;
		float distanceWithTarget = 0.0f;
		Vector3 currentPos = transform.position;
		
		// foreach related issue
		// Unity C#에서 foreach와 GC(Garbage Collection)
		// http://smilejp.tistory.com/82
		foreach(BaseCharacter baseCharacter in _characterManager._characterList)
		{
			if (baseCharacter._battleSide != _baseCharacter._battleSide)
			{
				if (targetCharacter == null)
				{
					targetCharacter = baseCharacter;
					distanceWithTarget = Vector3.Distance(currentPos, baseCharacter.transform.position);
					continue;
				}
				
				float distance = Vector3.Distance(currentPos, baseCharacter.transform.position);
				if (distance < distanceWithTarget)
				{
					targetCharacter = baseCharacter;
					distanceWithTarget = Vector3.Distance(currentPos, baseCharacter.transform.position);
				}
			}
		}
		
		return targetCharacter;
	}
}
