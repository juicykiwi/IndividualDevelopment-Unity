using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class BattleObject : ActionObject
{
	[SerializeField]
	protected AIControl _aiContorl = null;
	public AIControl AIControl { get { return _aiContorl; } }

	[SerializeField]
	protected  AnimationControl _animationControl = null;

    Dictionary<AIStateType, AnimationTrigger> _aiStateToAniTriggerDict =
        new Dictionary<AIStateType, AnimationTrigger>();

	[SerializeField]
    protected BattleModelControl _modelControl = null;
    public BattleModelControl ModelControl { get { return _modelControl; } }

    [SerializeField]
    protected VerticalMoveControl _verticalMoveControl = null;
    public VerticalMoveControl VerticalMoveControl { get { return _verticalMoveControl; } }

	[SerializeField]
	protected BattleStat _battleStat;
	public BattleStat BattleStat { get { return _battleStat; } }

	[SerializeField]
	protected BattleObjectInfoData _infoData = null;
	public BattleObjectInfoData InfoData 
	{ 
		get { return _infoData; } 
		set
		{
			_infoData = value; 
            _battleStat = (BattleStat)_infoData._battleStat.Clone();
		}
	}

	[SerializeField]
	protected int _createdId = 0;
	public int CreatedId
	{
		get { return _createdId; }
		set { _createdId = value; }
	}

    public virtual BattleTeam BattleTeam { get { return BattleTeam.None; } }

    // Method

    #region AI related

	public void InitAI()
	{
		// Add ai
        _aiContorl.AddAIState(this, new AIStateSpawn());
        _aiContorl.AddAIState(this, new AIStateIdle());
        _aiContorl.AddAIState(this, new AIStateMove());
        _aiContorl.AddAIState(this, new AIStateAttack());
        _aiContorl.AddAIState(this, new AIStateGetHit());
        _aiContorl.AddAIState(this, new AIStateDie());		
		
		// Set current ai
		_aiContorl.SetAIStateWithType(AIStateType.Spawn);

        // AIStateType to Animation trigger
        _aiStateToAniTriggerDict.Add(AIStateType.Spawn, AnimationTrigger.Idle);
        _aiStateToAniTriggerDict.Add(AIStateType.Idle, AnimationTrigger.Idle);
        _aiStateToAniTriggerDict.Add(AIStateType.Move, AnimationTrigger.Move);
        _aiStateToAniTriggerDict.Add(AIStateType.Jump, AnimationTrigger.Move);
        _aiStateToAniTriggerDict.Add(AIStateType.Attack, AnimationTrigger.Attack);
        _aiStateToAniTriggerDict.Add(AIStateType.Die, AnimationTrigger.Die);

		// Set event delegate
        _aiContorl.ChangeAINotifyEvent += OnChangedAIState;
	}

    public void OnChangedAIState(AIStateType type)
    {
        if (_aiStateToAniTriggerDict.ContainsKey(type) == false)
            return;

        _animationControl.SetAnimation(_aiStateToAniTriggerDict[type]);
    }

    #endregion

    public void InitStat()
    {
        _battleStat.InitCurrentStat();
    }

    public override void RemoveActionObject()
	{
        Destroy(gameObject);
	}

	#region Battle related

	public override bool IsEnableGetHit()
	{
		switch (AIControl.CurrentAIState.AIStateType)
		{
		case AIStateType.GetHit:
		case AIStateType.Die:
			return false;
		}

		return true;
	}

	public override void GetHit(BattleObject hitter)
	{
		AIControl.SetAIStateWithType(AIStateType.GetHit);
		AIStateGetHit aiStateGetHit = AIControl.CurrentAIState as AIStateGetHit;
		if (aiStateGetHit == null)
		{
            Debug.Log("GetHit(). aiStateGetHit is null.");
		}
		aiStateGetHit.AIStartSetting(hitter);
	}

	#endregion

	#region Collision

    protected List<ActionObject> GetCollisionActionObjectList()
    {
        List<ActionObject> hitActionObjectList = new List<ActionObject>();

        if (_modelControl.ColliderChecker_BattleObject == null)
            return hitActionObjectList;

        List<ModelControl> hitModelList = 
            _modelControl.ColliderChecker_BattleObject.GetHits<ModelControl>();

        if (hitModelList.Count > 0)
        {
            for (int index = 0; index < hitModelList.Count; ++index)
            {
                ModelControl model = hitModelList[index];
                if (model == null)
                    continue;

                if (model.RootActionObject == null)
                    continue;

                hitActionObjectList.Add(model.RootActionObject);
            }
        }

        return hitActionObjectList;
    }

	#endregion
}