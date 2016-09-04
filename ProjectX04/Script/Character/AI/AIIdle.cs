using UnityEngine;
using System.Collections;

public class AIIdle : AIBase {

	protected override void Awake ()
	{
		base.Awake ();
		
		_state = AIState.Idle;
	}

	protected override void FirstUpdate ()
	{
		base.FirstUpdate();
		
		aiController.ani.SetAni(AniTriggerType.Idle);
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		if (_fixedRunTime < aiController.cha.stat._idleWaitTime)
			return;

		aiController.SetState(AIState.Detect, null);
	}
}
