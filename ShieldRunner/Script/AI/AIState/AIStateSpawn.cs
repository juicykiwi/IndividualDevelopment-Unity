using UnityEngine;
using System.Collections;

public class AIStateSpawn : AIState
{
    // Method

	public AIStateSpawn()
	{
		_aiStateType = AIStateType.Spawn;
	}

    #region AIState override 

    protected override void StartAct()
    {
        BattleObject.VerticalMoveControl.gameObject.SetActive(true);
    }

	public override void Reason()
	{
        base.Reason();
		
        if (BattleObject.BattleTeam == BattleTeam.HeroTeam)
        {
            if (BattleObject.ModelControl.IsGround() == false)
            {
                return;
            }
        }

        if (_updateTime > 0.5f)
		{
			ChangeAIEvent(AIStateType.Idle);
		}
	}

    #endregion
}
