using UnityEngine;
using System.Collections;

public class AniChk : MonoBehaviour {

	BaseCharacter _baseCharacter;
	public bool _dummyChk = false;   //더미캐릭터 = false
	

	// Use this for initialization
	void Start () {
		_baseCharacter = transform.parent.GetComponent<BaseCharacter>();
	}	
	
	// Update is called once per frame
	void Update () {
	
	}

	void PreAttack()
	{
		if (!_dummyChk) {
			_baseCharacter.PreAttack();
			}
	}

	void Attack()
	{
		if (!_dummyChk) {
			_baseCharacter.OnAttackTrigger();
				}
	}

	void PostAttack()
	{
		if (!_dummyChk) {
			_baseCharacter.PostAttack();
				}
	}

	void HitStart()
	{
		_baseCharacter.OnHitStart();
	}

	void HitEnd()
	{
		_baseCharacter.OnHitEnd();
	}

	void HitReactionEnd_knockdownStart()
	{
		_baseCharacter.OnHitReactionEnd_knockdownStart();
	}

	void HitReactionEnd_airborneStart()
	{
		_baseCharacter.OnHitReactionEnd_airborneStart();
	}
	
	void Damage()
	{

		
	}
}
