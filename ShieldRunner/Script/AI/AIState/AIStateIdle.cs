using UnityEngine;
using System;
using System.Collections;

public class AIStateIdle : AIState
{
    // Method

	public AIStateIdle()
	{
		_aiStateType = AIStateType.Idle;
	}

    #region AIState override 

	public override void Reason()
	{
		base.Reason();

        if (BattleObject.BattleStat._currentSpeed == 0f)
            return;

		ChangeAIEvent(AIStateType.Move);
	}
	
	public override void Act()
	{
		base.Act();
	}

    #endregion
}
