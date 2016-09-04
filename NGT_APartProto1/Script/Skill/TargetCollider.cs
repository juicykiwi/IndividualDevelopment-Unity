using UnityEngine;
using System.Collections;

public class TargetCollider : MonoBehaviour {
	
	protected BaseCharacter _caster = null;
	public void SetCaster(BaseCharacter caster) { _caster = caster; }
	public BattleSkill _useSkill = null;

	protected BattleAction _battleAction = null;
	public void SetBattleAction(BattleAction battleAction) { _battleAction = battleAction; }
	
	public float _attackEndTime = 0.2f;
	public float _detroyObjectTime = 1.0f;
	protected bool _isAttackEnd = false;

	// Use this for initialization
	void Start () {
		Invoke("SetAttackEnd", _attackEndTime);

//		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetAttackEnd()
	{
		_isAttackEnd = true;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (_isAttackEnd == true)
			return;

		BaseCharacter target = collider.gameObject.GetComponent<BaseCharacter>();
		if (target == null)
			return;

		switch (_battleAction._actionMainType)
		{
		case ActionMainType.UseCollider:
		{
			if (target._battleSide == _caster._battleSide)
				return;

			if (_battleAction == null)
				return;
			
			target.SetDamage(_caster, _useSkill, _battleAction);
		}
			break;

		case ActionMainType.UseColliderForHeal:
		{
			if (target._battleSide != _caster._battleSide)
				return;
			
			if (_battleAction == null)
				return;
			
			target.SetHeal(_useSkill, _battleAction);
		}
			break;

		default:
		{
			return;
		}
		}
	}

//	void OnTriggerStay(Collider collider)
//	{
//		// 두 물체 간의 충돌이 지속될 때
//		//Debug.Log("TargetCollider :: OnTrigger Stay()");
////		Debug.Log(string.Format("{0}", collider.name));	
//	}
	
//	void OnTriggerExit(Collider collider)
//	{
//		// 두 물체가 서로 떨어졌을 때
//	}

//	void OnDestroy()
//	{
//	}
}
