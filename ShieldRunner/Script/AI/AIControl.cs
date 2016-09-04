using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum AIStateType
{
	None = 0,
	Spawn,
	Idle,
	Move,
	Jump,
	Attack,
	GetHit,
	Success,
	Die,
}

public class AIControl : MonoBehaviour
{	
	[SerializeField]
	AIState _currentAIState = null;
	public AIState CurrentAIState
	{
		set { _currentAIState = value; } 
		get { return _currentAIState; }
	}

    Dictionary<AIStateType, AIState> _aiDict = new Dictionary<AIStateType, AIState>();

	// Event
	public Action<AIStateType> ChangeAINotifyEvent = null;

    // Method

    #region MonoBehaviour event

	void Update()
	{
		CurrentAIState.Act();
        ReasonProcess();
	}

    #endregion

    void ReasonProcess()
    {
        if (CurrentAIState == null)
            return;

        CurrentAIState.Reason();
    }
        
	public void AddAIState(BattleObject battleObject, AIState state)
	{
		_aiDict[state.AIStateType] = state;
		_aiDict[state.AIStateType].BattleObject = battleObject;
		_aiDict[state.AIStateType].ChangeAIEvent += OnChangeAIEvent;
	}

	public AIState GetAIState(AIStateType type)
	{
		if (_aiDict.ContainsKey(type) == false)
			return null;

		return _aiDict[type];
	}

	public void SetAIStateWithType(AIStateType type)
	{
		AIState aiState = GetAIState(type);
		if (aiState == null)
		{
			Debug.LogError("SetAIStateWithType() : aiState == null");
			return;
		}

		if (aiState.AIStateType == CurrentAIState.AIStateType)
			return;

		if (CurrentAIState.IsExistNotChangeableAIStateType(type) == true)
			return;

		aiState.ClearValues();
		CurrentAIState = aiState;

		if (ChangeAINotifyEvent != null)
		{
			ChangeAINotifyEvent(CurrentAIState.AIStateType);
		}
	}

	public void OnChangeAIEvent(AIStateType type)
	{
		SetAIStateWithType(type);
	}
}





