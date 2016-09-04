using UnityEngine;
using System;
using System.Collections;

public enum AniTriggerType
{
	None,
	Idle,
	Move,
	Die,
}

public class AniController : MonoBehaviour {

	// Property

	Animator _animator = null;

	public Action _aniEndedAction = null;

	// Method

	void Awake () {
		_animator = this.GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate () {

	}

	public void SetAni(AniTriggerType triggerType)
	{
		for (AniTriggerType type = AniTriggerType.Idle; type <= AniTriggerType.Die; ++type)
		{
			_animator.SetBool(type.ToString(), false);
		}

		_animator.SetBool(triggerType.ToString(), true);
	}

	public void EventCallEnded()
	{
		if (_aniEndedAction == null)
			return;

		_aniEndedAction();
	}
}
