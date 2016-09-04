using UnityEngine;
using System.Collections;

public class BattleAction : MonoBehaviour {

	public int _damage = 0;
	public int _heal = 0;
	public float _actionOnTime = 0.0f;

	public GameObject _actionEffect = null;
	public float _actionEffectDestroyTime = 0.0f;

	public Material _hitMaterial = null;
	public float _hitMaterialActiveTime = 0.1f;

	public ActionMainType _actionMainType = ActionMainType.None;
	public ActionHitType _actionHitType = ActionHitType.Normal;
	public SkillDamageOnTiming _skillDamageOnTiming = SkillDamageOnTiming.AnimationEventTrigger;
	public ActionRangeSubType _actionRangeSubType = ActionRangeSubType.Normal;

	public GameObject _targetCollider = null;
	public GameObject _projectileObject = null;

	public Vector3 _positionForDamageOnTime = new Vector3();
	public Vector3 _scaleForDamageOnTime = new Vector3(1.0f, 1.0f, 1.0f);
	public Vector3 _rotateForObject = new Vector3(0.0f, 0.0f, 1.0f);

	public float _stunTime = 0.0f;
	public GameObject _stunEffect = null;

	public float _knockbackPower = 0.0f;

	public float _knockdownTime = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
