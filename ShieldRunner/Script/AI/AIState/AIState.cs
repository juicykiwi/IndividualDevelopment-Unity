using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class AIState
{
	[SerializeField]
	protected string _aiStateName = "";

	[SerializeField]
	protected AIStateType _aiStateType = AIStateType.None;
	public AIStateType AIStateType { get { return _aiStateType; } }

	[SerializeField]
	protected float _updateTime = 0f;

	[SerializeField]
	protected bool _firstUpdateCall = true;

	protected BattleObject _battleObejct = null;
	public BattleObject BattleObject
	{
		set { _battleObejct = value; }
		get { return _battleObejct; }
	}

	// Event
	public Action<AIStateType> ChangeAIEvent;

    // Not changable state related
    List<AIStateType> _notChangeableAIStateType = new List<AIStateType>();

    // Method

	public AIState()
	{
		_aiStateName = this.GetType().Name;
	}

	public virtual void ClearValues()
	{
        _updateTime = 0f;
		_firstUpdateCall = true;
	}

	public virtual void Reason()
	{
        _updateTime += Time.deltaTime;
	}

	protected virtual void StartAct()
	{
	}

	public virtual void Act()
	{
		if (_firstUpdateCall == true)
		{
			_firstUpdateCall = false;

			StartAct();
		}
	}

    #region Not changable state related

	protected void AddNotChangeableAIStateType(AIStateType type)
	{
		if (IsExistNotChangeableAIStateType(type) == true)
			return;

		_notChangeableAIStateType.Add(type);
	}

	public bool IsExistNotChangeableAIStateType(AIStateType type)
	{
        return (_notChangeableAIStateType.Contains(type));
	}

    #endregion
}
