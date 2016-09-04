using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AnimationTrigger
{
	None,
	Idle,
	Move,
	Jump,
	Attack,
	GetHit,
	Die,
	Success,
	Max,
}

public class AnimationControl : MonoBehaviour
{
	[SerializeField]
	Animator _animator = null;

    [SerializeField]
    AnimationTrigger _lastedTrueTiggerType = AnimationTrigger.Idle;

    // Method

	public void SetAnimation(AnimationTrigger trigger)
	{
        _animator.SetBool(_lastedTrueTiggerType.ToString(), false);

        _animator.SetBool(trigger.ToString(), true);
        _lastedTrueTiggerType = trigger;
	}
}
