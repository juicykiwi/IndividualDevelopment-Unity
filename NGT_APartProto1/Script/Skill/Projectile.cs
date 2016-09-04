using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	// 총알 발사 속도
	public Vector3 _direct = new Vector3(0.0f, 0.0f, 0.0f);
	public float _speed = 1000.0f;
	public float _lifeTime = 10.0f;
	public BaseCharacter _caster = null;
	public BattleSkill _useSkill = null;
	public BattleAction _battleAction = null;

	// Use this for initialization
	void Start () {

	}

	public void Fire()
	{
		GetComponent<Rigidbody>().AddForce(_direct * _speed);
	}
	
	// Update is called once per frame
	void Update () {
		_lifeTime -= Time.deltaTime;
		if (_lifeTime <= 0.0f)
		{
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		// 두 물체 간의 충돌이 일어나기 시작했을 때

		BaseCharacter target = collider.gameObject.GetComponent<BaseCharacter>();
		if (target == null)
			return;

		if (target._battleSide == _caster._battleSide)
			return;

		BattleAction action = _battleAction;
		if (action == null)
			return;

		target.SetDamage(_caster, _useSkill, action);

		if (action._actionRangeSubType == ActionRangeSubType.Normal)
		{
			Destroy(this.gameObject);
		}
	}

	/* Collision event
	 * 
	// isTrigger 옵션 : false
	void OnCollisionEnter()
	{
		// 두 물체 간의 충돌이 일어나기 시작했을 때
	}

	void OnCollisionStay()
	{
		// 두 물체 간의 충돌이 지속될 때
	}

	void OnCollisionExit()
	{
		// 두 물체가 서로 떨어졌을 때
	}

	// isTrigger 옵션 : true
	void OnTriggerEnter()
	{
		// 두 물체 간의 충돌이 일어나기 시작했을 때
	}

	void OnTriggerStay()
	{
		// 두 물체 간의 충돌이 지속될 때
	}

	void OnTriggerExit()
	{
		// 두 물체가 서로 떨어졌을 때
	}
	*/
}
