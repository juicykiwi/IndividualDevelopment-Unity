using UnityEngine;
using System.Collections;

// 아머타입
public enum ArmorType
{
	Normal,			// 일반
	LightArmor,		// 약히트 무시
	MiddleArmor,	// 강히트 무시
	HeavyArmor,		// 무거움
	Invincible,		// 무적
};

public class StatBase : MonoBehaviour {


	// HP
	public int _hpMax = 1;
	public int _hpCurrent = 1;
	// 이동 속도
	public float _moveSpeed = 1.0f;
	// 공격력
	public int _strength = 1;
	// 공격 속도
	public float _attackDelay = 0.0f;
	// 크리티컬 확률
	public float _criticalRatio = 0.0f;
	// 충돌 영역
	//public GameObject _hitBox = null;
	// 사정 거리
	//public float _attackRange = 0.0f;
	// 아머타입
//	public enum ArmorType
//	{
//		Normal,				// 일반
//		IgnoreLightHit,		// 약히트 무시
//		IgnoreStrongHit,	// 강히트 무시
//		Heavy,				// 무거움
//		Invincible,			// 무적
//	};
	public ArmorType _armorType = ArmorType.Normal;

	void Awake () {

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float GetHpCurrentRatio()
	{
		if (_hpCurrent <= 0)
			return 0.0f;

		return (float)_hpCurrent / (float)_hpMax;
	}
}
