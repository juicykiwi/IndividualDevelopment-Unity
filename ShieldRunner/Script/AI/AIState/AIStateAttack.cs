using UnityEngine;
using System;
using System.Collections;

public class AIStateAttack : AIState
{
    [SerializeField]
    float _attackEndTime = 0f;

    [SerializeField]
    AIStateType _nextAIStateType = AIStateType.None;

    // Method

	public AIStateAttack()
	{
		_aiStateType = AIStateType.Attack;
	}

    #region AIState override 

    public override void ClearValues()
    {
        base.ClearValues();

        _attackEndTime = 0f;
        _nextAIStateType = AIStateType.None;
    }

    public override void Act()
    {
        base.Act();

        Move();
    }

	public override void Reason()
	{
		base.Reason();

        if (_updateTime < _attackEndTime)
            return;

        if (_nextAIStateType == AIStateType.None)
        {
            ChangeAIEvent(AIStateType.Move);
            return;
        }

        ChangeAIEvent(_nextAIStateType);
	}

    #endregion

    public void AIStartSetting(float endTime, AIStateType nextAIStateType)
    {
        _attackEndTime = endTime;
        _nextAIStateType = nextAIStateType;
    }

    public void Move()
    {
        float speed = BattleObject.BattleStat._currentSpeed * Time.deltaTime;
        BattleObject.ModelControl.MoveFront(speed);
    }
}
