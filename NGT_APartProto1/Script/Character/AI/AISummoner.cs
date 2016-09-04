using UnityEngine;
using System.Collections;

public class AISummoner : BaseAI {

	protected override void Awake () {
		base.Awake ();
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (_baseCharacter.IsAlive() == false)
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
			BattleSkill useSkill = _baseCharacter.GetSkillController().GetUseSkillWithSlotIndex(SkillSlotType.BaseAttack);
			if (useSkill == null)
			{
				setAIState(AIState.Idle);
				break;
			}
			
			_baseCharacter.SetUseSkill(useSkill);
			setAIState(AIState.Attack);
			useSkill.ResetCoolTime();
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

			if (_baseCharacter._battleSide == BattleSide.A) {  //다잉 체크
				Debug.Log("ally summoner die");
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

	void UpdateLookAt()
	{
	}

	public void OnClickSkillIcon(SkillSlotType slotType)
	{
		switch (_aiState)
		{
		case AIState.Die:
		case AIState.PreAttack:
		case AIState.SkillAttack:
		{
			return;
		}
		default:
		{
		}
			break;
		}

		BattleSkill skill = _baseCharacter.GetSkillController().GetUseSkillWithSlotIndex(slotType);
		if (skill == null)
			return;

		if (skill._skillSlotType == SkillSlotType.BaseAttack)
			return;

		if (skill.GetRemainCoolTime() > 0)
			return;

		_baseCharacter.SetUseSkill(skill);

		if (skill._castTime < 0.5f)
		{
			setAIState(AIState.SkillAttack);
			skill.ResetCoolTime();
			return;
		}

		setAIState(AIState.PreAttack);
	}
}
