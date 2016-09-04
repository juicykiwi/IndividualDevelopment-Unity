using UnityEngine;
using System.Collections;

public class AIAttack : AIBase {

	protected override void Awake ()
	{
		base.Awake ();
		
		_state = AIState.Attack;
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		
		if (_fixedRunTime < aiController.cha.stat._attackSpeed)
			return;

		ChaController userCha = ChaManager.instance.GetUserCha();
		if (userCha == null)
			return;

		if (aiController.IsAttackRange(userCha.GetPos()) == false)
		{
			aiController.SetState(AIState.Idle, null);
			return;
		}

		userCha.GetHit(aiController.cha);

		aiController.SetState(AIState.Idle, null);
	}
}
