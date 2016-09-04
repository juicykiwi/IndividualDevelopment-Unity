using UnityEngine;
using System.Collections;

public class BaseAI : MonoBehaviour {

	public enum AIState
	{
		Idle,
		Move,
		MoveCommand,
		SkillSelect,
		PreAttack,
		Attack,
		SkillAttack,
		PostAttack,
		LightHit,
		MiddleHit,
		Stun,
		Knockback,
		KnockdownStart,
		KnockdownWait,
		KnockdownEnd,
		AirborneStart,
		Die,
	};

	public AIState _aiState = AIState.Idle;

	public float idleTimeBeforeBattleStart = 0.0f;

	protected Transform _transform;
	protected BaseCharacter _baseCharacter;
	protected CharacterManager _characterManager;
	protected CreatCharacter _createCharacter;

	protected CharacterAniController _aniController;

	protected Vector3 _destPos = new Vector3 (0.0f, 0.0f, 0.0f);

	protected bool _isAttackDelay = false;

	protected virtual void Awake () {
		// Generic casting type
		_transform = GetComponent<Transform>();

		// Generic casting type
//		_transform = GetComponent<Transform>();

		// etc casting type
//		_transform = gameObject.GetComponent<Transform>();
//		_transform = (Transform)GetComponent("Transform");
//		_transform = (Transform)GetComponent(typeof(Transform));

		_baseCharacter = GetComponent<BaseCharacter>();
		_characterManager = FindObjectOfType<CharacterManager>();
		_createCharacter = FindObjectOfType<CreatCharacter> ();

		_aniController = this.gameObject.AddComponent<CharacterAniController>();

		_destPos = _transform.position;
	}

	void Start () {

	}

	void Update () {

	}

	public virtual void setAIState(AIState aiState)
	{
	}

	protected void SkillCastComplete()
	{
		setAIState(AIState.SkillAttack);
	}
	
	protected void SkillCastFail()
	{
		CancelInvoke("SkillCastComplete");
	}

	public void ActiveAttackDelay()
	{
		CancelInvoke("DeactivateAttackDelay");
		
		_isAttackDelay = true;
		Invoke ("DeactivateAttackDelay", _baseCharacter.getStat()._attackDelay);
	}
	
	protected void DeactivateAttackDelay()
	{
		_isAttackDelay = false;
	}

	public void DeactivateStun()
	{
		setAIState(AIState.Idle);
	}

	public void DeactivateKnockdown()
	{
		setAIState(AIState.KnockdownEnd);
	}
}

