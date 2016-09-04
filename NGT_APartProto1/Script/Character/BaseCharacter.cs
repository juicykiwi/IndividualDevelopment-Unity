using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CharacterMainType
{
	Summoner,
	Creature,
};

public enum BattleSide
{
	None,
	A,
	B,
};

public class BaseCharacter : MonoBehaviour {

	public CharacterMainType _mainType = CharacterMainType.Creature;
	public BattleSide _battleSide = BattleSide.None;
	public int _unitIndex ;

	protected StatBase _stat = null;
	public StatBase getStat() { return _stat; }

	protected float _attackRange = 1.0f;
	public float getAttackRange() { return _attackRange; }

	protected BaseCharacter _targetCharacter = null;
	public BaseCharacter GetTargetCharacter() { return _targetCharacter;} 
	public void SetTargetCharacter(BaseCharacter target) { _targetCharacter = target;} 

	protected BattleSkill _useSkill = null;
	public void SetUseSkill(BattleSkill skill) { _useSkill = skill; }
	public BattleSkill GetUseSkill() { return _useSkill; }

	protected BaseAI _baseAI = null;

	protected SkillController _skillController = null;
	public SkillController GetSkillController() {return _skillController;}

	// 히트 이펙트 관련 처리 : 코드 개선 필요!!!
	protected bool _isHitColor = false;
	protected float _remainHitColorTime = 0.0f;
	protected SkinnedMeshRenderer _smr = null;
	protected Material _tagBodyMaterial = null;
	protected Material _tagBodyMaterialOrigin = null;

	// HUD 데미지 수치
	protected DamageNumeric _damageNumeric = null;

	// HUD HP 바
	protected HUDHpController _hudHpController = null;

	// 캐스팅 이펙트 오브젝트
	protected GameObject _castEffectObject = null;

	public Vector3 _forceMoveDest = new Vector3();
		
	void Awake () {
		// 스크립트가 실행될 때 한 번만 호출되는 함수다.
		// 주로 게임의 상태 값 또는 변수의 초기화에 사용한다.
		// Start 함수가 호출괴지 전에 맨 먼저 호출된다.
		// 스크립트가 비활성화돼 있어도 실행된다.
		// Coroutine 사용 불가

		_stat = this.GetComponent<StatBase>();
		_baseAI = this.GetComponent<BaseAI>();
		_skillController = this.GetComponent<SkillController> ();

		// 피격 처리
		ArrayList list = new ArrayList ();
		list.AddRange (this.GetComponentsInChildren<SkinnedMeshRenderer> ());
		
		for (int i = 0; i < list.Count; ++i)
		{
			SkinnedMeshRenderer smr = (SkinnedMeshRenderer)list[i];
			if (smr)
			{
				if (smr.CompareTag("Tag_CharacterBody") == true)
				{
					_smr = smr;
					_tagBodyMaterialOrigin = _smr.material;
					break;
				}
			}
		}

		_damageNumeric = this.GetComponentInChildren<DamageNumeric>();

		_hudHpController = this.GetComponentInChildren<HUDHpController>();
	}
	
	// Use this for initialization
	void Start () {
		// Update 함수가 호출되기 전에 한 번만 호출된다.
		// 스크립트가 활성화돼 있어야 실행된다.
		// 다른 스크립트의 모든 Awake가 모두 다 실행된 이후에 실행된다.

		BattleSkill battleSkill = _skillController._skillList[0];
		if (battleSkill)
		{
			_attackRange = battleSkill._range;
		}
		else
		{
			_attackRange = 1.0f;
		}
	}
	
	// Update is called once per frame
	public void Update () {
		// 프레임마다 호출되는 함수로 주로 게임의 핵심 로직을 작성한다.
		// 스크립트가 활성화돼 있어야 실행된다.

		if (_isHitColor == true)
		{
			_remainHitColorTime -= Time.deltaTime;
			if (_remainHitColorTime < 0.0f)
			{
				if (_smr)
				{
					_smr.material = _tagBodyMaterialOrigin;
				}
				_isHitColor = false;
			}
		}
	}

	public void ActiveCastingEffect()
	{
		if (_useSkill == null)
			return;

		if (_useSkill._castingEffect == null)
			return;

		Vector3 effectPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		_castEffectObject = (GameObject)Instantiate(_useSkill._castingEffect, effectPos, transform.rotation);
		Destroy(_castEffectObject, _useSkill._castingEffectDestroyTime);
	}

	public void DeactivateCastingEffect()
	{
		if (_castEffectObject)
		{
			Destroy(_castEffectObject);
		}
	}

	public void PreAttack() {

		List<BattleAction> damageOnTimeActions = new List<BattleAction>();

		for (int i = 0; i < _useSkill.GetBattleActions().Length; ++i)
		{
			BattleAction action = _useSkill.GetBattleActions()[i];
			if (action == null)
				continue;

			if (action._skillDamageOnTiming == SkillDamageOnTiming.UseDamageOnTime)
			{
				damageOnTimeActions.Add(action);
			}
		}

		if (damageOnTimeActions.Count > 0)
		{
			StartCoroutine("OnAttackDamageOnTime", damageOnTimeActions);
		}
	}

	public void OnAttackTrigger() {

		for (int i = 0; i < _useSkill.GetBattleActions().Length; ++i)
		{
			BattleAction action = _useSkill.GetBattleActions()[i];
			if (action == null)
				continue;

			if (action._skillDamageOnTiming == SkillDamageOnTiming.UseDamageOnTime)
				continue;

			Attack(action);
		}
	}

	IEnumerator OnAttackDamageOnTime(List<BattleAction> damageOnTimeActions)
	{
		while(damageOnTimeActions.Count > 0)
		{
			yield return new WaitForSeconds(damageOnTimeActions[0]._actionOnTime);

			BattleAction currentAction = damageOnTimeActions[0];
			if (currentAction)
			{
				Attack(currentAction);
			}
			
			damageOnTimeActions.RemoveAt(0);
			
			if (damageOnTimeActions.Count <= 0)
				break;
		}

		yield return null;
	}

	public void Attack(BattleAction action) {

		if (_targetCharacter == null)
			return;

		if (_useSkill == null)
			return;

		if (action == null)
			return;

		switch (action._actionMainType)
		{

		case ActionMainType.Melee:
		{
			_targetCharacter.SetDamage(this, _useSkill, action);
		}
			break;

		case ActionMainType.UseCollider:
		{
			if (action._targetCollider == null)
				break;

			Quaternion rotate = Quaternion.LookRotation(transform.forward);
			Vector3 pos = transform.position + (rotate * action._positionForDamageOnTime);

			GameObject targetColliderObj = (GameObject)Instantiate(action._targetCollider, pos, transform.rotation);
			if (targetColliderObj == null)
				break;

			targetColliderObj.transform.localScale = action._scaleForDamageOnTime;

			TargetCollider targetCollider = targetColliderObj.GetComponent<TargetCollider>();
			if (targetCollider == null)
				break;

			targetCollider.SetCaster(this);
			targetCollider._useSkill = _useSkill;
			targetCollider.SetBattleAction(action);

			Destroy(targetColliderObj, targetCollider._detroyObjectTime);
		}
			break;

		case ActionMainType.Range:
		{
			if (action._projectileObject == null)
				break;

			Quaternion rotate = Quaternion.LookRotation(transform.forward);
			Vector3 pos = transform.position + (rotate * action._projectileObject.transform.position);
			GameObject projectileObj = (GameObject)Instantiate(action._projectileObject, pos, transform.rotation);
			if (projectileObj == null)
				break;

			Projectile projectile = projectileObj.GetComponent<Projectile>();
			if (projectile == null)
				break;

			Quaternion directRotate = Quaternion.LookRotation(action._rotateForObject);
			Vector3 direct = Vector3.Normalize(_targetCharacter.transform.position - transform.position);
			direct = directRotate * direct;
			projectile._direct = direct;
			projectile._caster = this;
			projectile._useSkill = _useSkill;
			projectile._battleAction = action;
			projectile.Fire();
		}
			break;

		case ActionMainType.Heal:
		{
			BaseCharacter target = CharacterManager.GetInstance().LowHpRatioCharacter(_battleSide);
			if (target == null)
				return;

			target.SetHeal(_useSkill, action);
		}
			break;

		case ActionMainType.UseColliderForHeal:
		{
			if (action._targetCollider == null)
				break;
			
			Quaternion rotate = Quaternion.LookRotation(transform.forward);
			Vector3 pos = transform.position + (rotate * action._positionForDamageOnTime);
			
			GameObject targetColliderObj = (GameObject)Instantiate(action._targetCollider, pos, transform.rotation);
			if (targetColliderObj == null)
				break;
			
			targetColliderObj.transform.localScale = action._scaleForDamageOnTime;
			
			TargetCollider targetCollider = targetColliderObj.GetComponent<TargetCollider>();
			if (targetCollider == null)
				break;
			
			targetCollider.SetCaster(this);
			targetCollider._useSkill = _useSkill;
			targetCollider.SetBattleAction(action);
			
			Destroy(targetColliderObj, targetCollider._detroyObjectTime);
		}
			break;
			
		default:
		{
			break;
		}
		}

	}

	public void PostAttack()
	{
		_baseAI.setAIState(BaseAI.AIState.PostAttack);
	}

	public void SetDamage(BaseCharacter caster, BattleSkill useSkill, BattleAction action)
	{
		int criHit = 1;
		if (Random.Range (0.0f,1.0f)<=_stat._criticalRatio)
		{
			criHit = 2;
		}

		float increaseValue = SkillManager.GetInstance()._skillLevelData.IncreaseValueWithSkillLevel(useSkill._skillLevel);

		//_stat._hpCurrent -= action._damage;
		int damCurrent = (int)(Mathf.Round (action._damage * increaseValue * Random.Range (0.9f,1.1f) * criHit)) ;  //CriticalRatio 및 랜덤값 적용
		_stat._hpCurrent -= damCurrent;

		if (action._actionEffect != null)
		{
			Vector3 effectPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			effectPos.y = 2.0f;
			Object hitEffectObject = Instantiate(action._actionEffect, effectPos, transform.rotation);
			Destroy(hitEffectObject, action._actionEffectDestroyTime);
		}

		if (action._hitMaterial)
		{
			if (_smr)
			{
				_smr.material = action._hitMaterial;
				_remainHitColorTime = action._hitMaterialActiveTime;
				_isHitColor = true;
			}
		}

		NumericType numericType = NumericType.NormalDamage;
		if (criHit == 2)
			numericType = NumericType.CriDamage;
		_damageNumeric.print(damCurrent, numericType);

		float hpRatio = (float)_stat._hpCurrent / (float)_stat._hpMax;
		_hudHpController.SetHpBarFillAmount(hpRatio);

		if (IsAlive() == true)
		{
			processHitReaction(caster, action);
		}
		else
		{
			_targetCharacter = null;
			_baseAI.setAIState(BaseAI.AIState.Die);
		}
	}

	public void SetHeal(BattleSkill useSkill, BattleAction action)
	{
		if (action == null)
			return;

		if (action._actionEffect != null)
		{
			Vector3 effectPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			effectPos.y = 2.0f;
			Object hitEffectObject = Instantiate(action._actionEffect, effectPos, transform.rotation);
			Destroy(hitEffectObject, action._actionEffectDestroyTime);
		}
		
		if (action._hitMaterial)
		{
			if (_smr)
			{
				_smr.material = action._hitMaterial;
				_remainHitColorTime = action._hitMaterialActiveTime;
				_isHitColor = true;
			}
		}

		float increaseValue = SkillManager.GetInstance()._skillLevelData.IncreaseValueWithSkillLevel(useSkill._skillLevel);

		int heal = (int)(action._heal * increaseValue);
		int hp = getStat()._hpCurrent + heal;
		getStat()._hpCurrent = Mathf.Min(hp, getStat()._hpMax);

		_damageNumeric.print(heal, NumericType.Heal);

		float hpRatio = (float)_stat._hpCurrent / (float)_stat._hpMax;
		_hudHpController.SetHpBarFillAmount(hpRatio);
	}

	public void OnPlayerSkill(SkillSlotType slotType)
	{
		Debug.Log (string.Format ("{0}", (int)slotType));

		if (_baseAI.GetType().ToString() != "AISummoner")
			return;

		((AISummoner)_baseAI).OnClickSkillIcon(slotType);
	}

	public bool IsAlive()
	{
		return (_stat._hpCurrent > 0);
	}

	public bool IsActiveHitReaction(ArmorType armorType, ActionHitType actionHitType)
	{
		bool isActiveHetAction = false;

		switch (armorType)
		{
		case ArmorType.Normal:
		{
			if (actionHitType >= ActionHitType.LightHit)
				isActiveHetAction = true;
		}
			break;

		case ArmorType.LightArmor:
		{
			if (actionHitType >= ActionHitType.MiddleHit)
				isActiveHetAction = true;
		}
			break;

		case ArmorType.MiddleArmor:
		{
			if (actionHitType >= ActionHitType.Stun)
				isActiveHetAction = true;
		}
			break;

		default:
		{
			isActiveHetAction = false;
		}
			break;

		}

		return isActiveHetAction;
	}

	public void processHitReaction(BaseCharacter caster, BattleAction action)
	{
		ArmorType armorType = _stat._armorType;
		ActionHitType actionHitType = action._actionHitType;
		
		if (IsActiveHitReaction(armorType, actionHitType) == false)
		{
			return;
		}

		switch (actionHitType)
		{
		case ActionHitType.Normal:
		{
		}
			break;

		case ActionHitType.LightHit:
		{
			if (_baseAI._aiState == BaseAI.AIState.Stun ||
			    _baseAI._aiState == BaseAI.AIState.Knockback ||
			    _baseAI._aiState == BaseAI.AIState.KnockdownStart ||
			    _baseAI._aiState == BaseAI.AIState.KnockdownWait ||
			    _baseAI._aiState == BaseAI.AIState.KnockdownEnd ||
			    _baseAI._aiState == BaseAI.AIState.AirborneStart)
				break;

			_baseAI.setAIState(BaseAI.AIState.LightHit);
		}
			break;

		case ActionHitType.MiddleHit:
		{
			if (_baseAI._aiState == BaseAI.AIState.Stun ||
			    _baseAI._aiState == BaseAI.AIState.Knockback ||
			    _baseAI._aiState == BaseAI.AIState.KnockdownStart ||
			    _baseAI._aiState == BaseAI.AIState.KnockdownWait ||
			    _baseAI._aiState == BaseAI.AIState.KnockdownEnd ||
			    _baseAI._aiState == BaseAI.AIState.AirborneStart)
				break;

			_baseAI.setAIState(BaseAI.AIState.MiddleHit);
		}
			break;

		case ActionHitType.Stun:

			if (_baseAI._aiState == BaseAI.AIState.AirborneStart)
				break;
		{
			_baseAI.setAIState(BaseAI.AIState.Stun);
			_baseAI.Invoke("DeactivateStun", action._stunTime);

			if (action._stunEffect)
			{
				GameObject stunEffectObject = (GameObject)Instantiate(action._stunEffect, transform.position, transform.rotation);
				stunEffectObject.transform.SetParent(this.transform);
				Destroy(stunEffectObject, action._stunTime);
			}
		}
			break;

		case ActionHitType.Knockback:
		{
			if (_baseAI._aiState == BaseAI.AIState.Stun ||
			    _baseAI._aiState == BaseAI.AIState.KnockdownStart ||
			    _baseAI._aiState == BaseAI.AIState.KnockdownWait ||
			    _baseAI._aiState == BaseAI.AIState.KnockdownEnd ||
			    _baseAI._aiState == BaseAI.AIState.AirborneStart)
				break;

			_baseAI.setAIState(BaseAI.AIState.Knockback);

			Vector3 direct = (-1.0f * Vector3.Normalize(caster.transform.position - transform.position));
			GetComponent<Rigidbody>().AddForce(direct * action._knockbackPower);
		}
			break;

		case ActionHitType.Knockdown:
		{
			if (_baseAI._aiState == BaseAI.AIState.Stun ||
			    _baseAI._aiState == BaseAI.AIState.KnockdownStart ||
			    _baseAI._aiState == BaseAI.AIState.KnockdownWait ||
			    _baseAI._aiState == BaseAI.AIState.KnockdownEnd ||
			    _baseAI._aiState == BaseAI.AIState.AirborneStart)
				break;

			_baseAI.setAIState(BaseAI.AIState.KnockdownStart);
			_baseAI.CancelInvoke("DeactivateKnockdown");
			_baseAI.Invoke("DeactivateKnockdown", action._knockdownTime);

			Vector3 direct = (-1.0f * Vector3.Normalize(caster.transform.position - transform.position));
			GetComponent<Rigidbody>().AddForce(direct * action._knockbackPower);
		}
			break;

		case ActionHitType.Airborne:
		{
			if (_baseAI._aiState == BaseAI.AIState.Stun)
				break;

			_baseAI.setAIState(BaseAI.AIState.AirborneStart);
			_baseAI.CancelInvoke("DeactivateKnockdown");
			_baseAI.Invoke("DeactivateKnockdown", action._knockdownTime);

			Vector3 direct = (-1.0f * Vector3.Normalize(caster.transform.position - transform.position));
			GetComponent<Rigidbody>().AddForce(direct * action._knockbackPower);
		}
			break;

		default:
		{
		}
			break;
		}
	}

	public void OnHitStart()
	{
	}

	public void OnHitEnd()
	{
		_baseAI.ActiveAttackDelay();
		_baseAI.setAIState(BaseAI.AIState.Idle);
	}

	public void OnHitReactionEnd_knockdownStart()
	{
		_baseAI.setAIState(BaseAI.AIState.KnockdownWait);
	}

	public void OnHitReactionEnd_airborneStart()
	{
		_baseAI.setAIState(BaseAI.AIState.KnockdownWait);
	}

	public void SetForceMoveDest(Vector3 destPos)
	{
		if (_baseAI._aiState == BaseAI.AIState.Stun ||
		    _baseAI._aiState == BaseAI.AIState.KnockdownStart ||
		    _baseAI._aiState == BaseAI.AIState.KnockdownWait ||
		    _baseAI._aiState == BaseAI.AIState.KnockdownEnd ||
		    _baseAI._aiState == BaseAI.AIState.AirborneStart)
		{
			return;
		}

		// 지금 현재 위치와 큰 차이가 없으면 리턴
		float distance = Vector3.Distance(transform.position, destPos);
		if (distance < 2.0f)
			return;

		_forceMoveDest = destPos;
		_baseAI.setAIState(BaseAI.AIState.MoveCommand);
	}

	public float DistanceTarget()
	{
		if (_targetCharacter == null)
			return 0.0f;

		Vector3 targetPos = _targetCharacter.transform.position;
		return Vector3.Distance(transform.position, targetPos);
	}

	public void UpdateLookAt()
	{
		if (_targetCharacter == null)
			return;

		Vector3 targetPos = _targetCharacter.transform.position;
		Vector3 dirNormal = Vector3.Normalize(targetPos - transform.position);

		// look at
		Quaternion rotation = Quaternion.LookRotation(dirNormal);
		transform.rotation = rotation;

		Vector3 destPos = new Vector3();
		transform.Translate(destPos, Space.World);
	}

	public void MoveToTarget()
	{
		if (_targetCharacter == null)
			return;

		Vector3 targetPos = _targetCharacter.transform.position;

		// direction for destination
		Vector3 dirNormal = Vector3.Normalize(targetPos - transform.position);

		// look at
		Quaternion rotation = Quaternion.LookRotation(dirNormal);
		transform.rotation = rotation;
		//			_transform.Rotate(...);
		
		// coordinate info
		// - local coordinate : Space.Self
		// - world coordinate : Space.World
		
		// move
		//			Vector3 newPostion = _transform.position + (dirNormal * _baseCharacter._moveSpeed * Time.deltaTime);
		//			_transform.position = newPostion;
		transform.Translate(transform.forward * _stat._moveSpeed * Time.deltaTime, Space.World);
	}

	public void MoveToForcePosDest()
	{
		Vector3 dirNormal = Vector3.Normalize(_forceMoveDest - transform.position);
		
		Quaternion rotation = Quaternion.LookRotation(dirNormal);
		transform.rotation = rotation;
		
		transform.Translate(transform.forward * _stat._moveSpeed * Time.deltaTime, Space.World);
	}

	/* 기본 스크립트 함수 정보
	void Awake () {
		// 스크립트가 실행될 때 한 번만 호출되는 함수다.
		// 주로 게임의 상태 값 또는 변수의 초기화에 사용한다.
		// Start 함수가 호출괴지 전에 맨 먼저 호출된다.
		// 스크립트가 비활성화돼 있어도 실행된다.
		// Coroutine 사용 불가
	}

	void Start () {
		// Update 함수가 호출되기 전에 한 번만 호출된다.
		// 스크립트가 활성화돼 있어야 실행된다.
		// 다른 스크립트의 모든 Awake가 모두 다 실행된 이후에 실행된다.
	}

	public void Update () {
		// 프레임마다 호출되는 함수로 주로 게임의 핵심 로직을 작성한다.
		// 스크립트가 활성화돼 있어야 실행된다.
	}
		
	void LateUpdate () {
		// 모든 Update 함수가 호출되고 나서 한 번씩 호출된다.
		// 순차적으로 실행해야 하는 로직에 사용한다.
		// 카메라 이동 로직에 주로 사용하는 함수다.
		// 스크립트가 활성화돼 있어야 실행된다.
	}

	void FixedUpdate () {
		// 주로 물리 엔진을 사용하는 경우에 일정 시간 간격으로 힘을 가할 때 사용하는 함수다.
		// 발생하는 주기가 일정하다.
	}
	
	void OnEnable () {
		// 게임오브젝트 또는 스크립트가 활성화됐을 때 호출된다.
		// Event 연결 시 사용한다.
		// Coroutine 사용 불가.
	}
	
	void OnDisable () {
		// 게임오브젝트 또는 스크립트가 비활성화돘을 때 호출된다.
		// Event 연결을 종료할 때 사용한다.
		// Coroutine 사용 불가.
	}
	
	void OnGUI () {
		// GUI 관련 함수를 사용할 때 사용한디.
	}
	*/
}
