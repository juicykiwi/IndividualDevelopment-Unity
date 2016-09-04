using UnityEngine;
using System;
using System.Collections;

public class AIStateMove : AIState
{
    // Method

	public AIStateMove()
	{
		_aiStateType = AIStateType.Move;
	}

    #region AIState override 

	public override void Act()
	{
		base.Act();

		Move();
	}

    #endregion

	public void Move()
	{
		float speed = BattleObject.BattleStat._currentSpeed * Time.deltaTime;
		BattleObject.ModelControl.MoveFront(speed);
	}
}
