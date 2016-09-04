using UnityEngine;
using System.Collections;

public class AIDieForUser : AIBase {

	// Method

	protected override void Awake ()
	{
		base.Awake ();

		_state = AIState.Die;
	}

	protected override void FirstUpdate ()
	{
		base.FirstUpdate();

		aiController.ani.SetAni(AniTriggerType.Die);

		aiController.ani._aniEndedAction += DieEndedAction;
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}

	public override bool IsPossibleNextAIState(AIState aiState)
	{
		return false;
	}

	public void DieEndedAction()
	{
		aiController.ani._aniEndedAction -= DieEndedAction;

		if (EventManager.instance._actionFailMission != null)
		{
			EventManager.instance._actionFailMission();
		}
	}
}
