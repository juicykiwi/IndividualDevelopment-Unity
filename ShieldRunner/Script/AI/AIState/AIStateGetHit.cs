using UnityEngine;
using System.Collections;

public class AIStateGetHit : AIState
{
	const float ReChangeIdleTime = 3f;
	const float GetHitFlySpeedRatio = 3f;

	BattleObject _hitter = null;
	
    // Method

	public AIStateGetHit()
	{
		_aiStateType = AIStateType.GetHit;
	}
        
	#region AIState override 

	public override void ClearValues()
	{
		base.ClearValues ();

		_hitter = null;
	}

	public override void Reason()
	{
		base.Reason ();

        if (_updateTime > ReChangeIdleTime)
		{
			ChangeAIEvent(AIStateType.Idle);
		}
	}

	protected override void StartAct()
	{
		base.StartAct();

		if (BattleObject.BattleStat.IsDead == true)
		{
			Vector3 hitterPos = _hitter.transform.position;
			float hitterSpeed = Mathf.Max(_hitter.BattleStat._currentSpeed, 1f);
			float getHitFlySpeed = hitterSpeed * GetHitFlySpeedRatio;

			ChangeAIEvent(AIStateType.Die);
			AIStateDie aiStateDie = BattleObject.AIControl.CurrentAIState as AIStateDie;
			aiStateDie.AIStartSetting(BattleObject.transform.position, hitterPos, getHitFlySpeed);

            if (BattleObject.BattleTeam == BattleTeam.MonsterTeam)
            {
                int rewardGold = BattleObject.InfoData._rewardGold;
                PlayerDataManager.instance.IncreaseGold(rewardGold);
            }
		}
	}

	#endregion

	public void AIStartSetting(BattleObject hitter)
	{
		_hitter = hitter;
	}
}
