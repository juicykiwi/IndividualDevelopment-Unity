using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIMove : AIBase {

	protected override void Awake ()
	{
		base.Awake ();
		
		_state = AIState.Move;
	}

	protected override void FirstUpdate ()
	{
		base.FirstUpdate();

		AIMessageMove message = _aiMessage as AIMessageMove;
		if (message == null)
		{
			aiController.SetState(AIState.Idle, null);
			return;
		}
		
		if (message._direction == Direction.None)
		{
			aiController.SetState(AIState.Idle, null);
			return;
		}

		aiController.cha.Move(message._direction, CompleteMove);
		
		aiController.ani.SetAni(AniTriggerType.Move);
	}

	public void CompleteMove()
	{
		aiController.SetState(AIState.Idle, null);
	}

	public override bool IsPossibleNextAIState(AIState aiState)
	{
		if (aiState == this.state)
			return false;

		return true;
	}
}
