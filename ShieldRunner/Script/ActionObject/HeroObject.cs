using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroObject : BattleObject
{
	// Created ID
	static int _createdHeroIdLast = 0;
	public static int GetNextCreatedHeroId { get { return ++_createdHeroIdLast; } }
	public static void ClearCreatedHeroId() { _createdHeroIdLast = 0; } 

    public override BattleTeam BattleTeam { get { return BattleTeam.HeroTeam; } }

	// Command
	[SerializeField]
	bool _moveUpCommand = true;
	public bool MoveUpCommand
	{
		get { return _moveUpCommand; }
	}

    // Battle process time
    float _battleProcessPeriodTime = 0.05f;
    float _currentBattleProcessTime = 0f;

    // Method

	#region ActionObject override

    public override void RemoveActionObject()
	{
		HeroManager.instance.RemoveCreatedHeroInDict(CreatedId);
		
		Destroy(gameObject);
	}

	#endregion

    #region MonoBehaviour event

    void Update()
    {
        _currentBattleProcessTime += Time.deltaTime;

        if (_currentBattleProcessTime >= _battleProcessPeriodTime)
        {
            _currentBattleProcessTime = 0f;

            if (IsEnableBattleProcess() == true)
            {
                BattleProcess();
            }
        }
    }
	
	#endregion

	#region Select

	public void SelectHero()
	{
		HeroManager.instance.SelectHeroObejct = this;
	}

	#endregion

    #region 이동 거리

    public float CurrentMoveDistance()
    {
        return transform.position.x;
    }

    #endregion

	#region Battle related

	bool IsEnableBattleProcess()
	{
		switch (_aiContorl.CurrentAIState.AIStateType)
		{
		case AIStateType.Spawn:
		case AIStateType.GetHit:
		case AIStateType.Die:
			return false;
		}

		return true;
	}

	void BattleProcess()
	{
        List<ActionObject> actionObjectList = GetCollisionActionObjectList();
        if (actionObjectList.Count <= 0)
            return;

        for (int index = 0; index < actionObjectList.Count; ++index)
        {
            ActionObject actionObject = actionObjectList[index];

            BattleObject battleObject = actionObject as BattleObject;
            if (battleObject != null)
            {
                if (battleObject.BattleTeam != BattleTeam.MonsterTeam)
                    continue;

                if (battleObject.IsEnableGetHit() == false)
                    continue;

                if (ModelControl.EquipItemSlotShield.EquipItem == null)
                {
                    GetHit(battleObject);
                    break;
                }

                if (ModelControl.EquipItemSlotShield.EquipItem.IsBroken() == true)
                {
                    GetHit(battleObject);
                    break;
                }

                Attack(battleObject);
            }

            PickupItemObject pickupItemObject = actionObject as PickupItemObject;
            if (pickupItemObject != null)
            {
                pickupItemObject.GetHit(this);
                continue;
            }
        }
	}

	void Attack(BattleObject battleObject)
	{
        if (ModelControl.EquipItemSlotShield.EquipItem != null)
        {
            ModelControl.EquipItemSlotShield.EquipItem.ReduceDurability();
            if (ModelControl.EquipItemSlotShield.EquipItem.IsBroken() == true)
            {
                ModelControl.EquipItemSlotShield.Unequip();
            }
        }

		battleObject.GetHit(this);

        AIControl.SetAIStateWithType(AIStateType.Attack);
        AIStateAttack aiStateAttack = AIControl.CurrentAIState as AIStateAttack;
        if (aiStateAttack == null)
            return;

        aiStateAttack.AIStartSetting(0.15f, AIStateType.Move);
	}

	public override void GetHit(BattleObject battleObject)
	{
        if (ModelControl.EquipItemSlotShield.EquipItem == null)
        {
            base.GetHit(battleObject);
            return;
        }
            
        ModelControl.EquipItemSlotShield.EquipItem.ReduceDurability();
        if (ModelControl.EquipItemSlotShield.EquipItem.IsBroken() == true)
        {
            ModelControl.EquipItemSlotShield.Unequip();
        }
	}

	#endregion

	#region Command

	public void SetMoveUpCommand(bool command)
	{
		if (command == true)
		{
			CancelInvoke("CancelMoveUpCommand");
			_moveUpCommand = true;
		}
		else
		{
			Invoke("CancelMoveUpCommand", 0.1f);
		}
	}

	public void CancelMoveUpCommand()
	{
		_moveUpCommand = false;
	}
        
    #endregion
}
