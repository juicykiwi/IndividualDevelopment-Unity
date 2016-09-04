using UnityEngine;
using System.Collections;

public class AISpawn : AIBase {

	protected override void Awake ()
	{
		base.Awake ();
		
		_state = AIState.Spawn;
	}

	protected override void OnEnable () {
		base.OnEnable();

		aiController.SetState(AIState.Idle, null);
	}
}
