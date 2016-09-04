using UnityEngine;
using System;
using System.Collections;

public class AIStateDie : AIState
{
	const float ScaleSpeed = 0.8f;
	const float RotateSpeed = 1440f;
	const float rotateAngle = 45f;
	
	float _flySpeed = 0f;
	
	Vector2 _originPos = Vector2.zero;
	Vector2 _hitterPos = Vector2.zero;

    // Method

	public AIStateDie()
	{
		_aiStateType = AIStateType.Die;

		AddNotChangeableAIStateType(AIStateType.Idle);
		AddNotChangeableAIStateType(AIStateType.Move);
		AddNotChangeableAIStateType(AIStateType.Jump);
		AddNotChangeableAIStateType(AIStateType.Attack);
		AddNotChangeableAIStateType(AIStateType.GetHit);
		AddNotChangeableAIStateType(AIStateType.Success);
	}

    #region AIState override 

	public override void ClearValues()
	{
		base.ClearValues ();
		
		_originPos = Vector2.zero;
		_hitterPos = Vector2.zero;
	}

	public override void Reason()
	{
		base.Reason();

        if (_updateTime > 1f)
		{
            BattleObject.RemoveActionObject();
		}
	}

	protected override void StartAct()
	{
		base.StartAct();

        BattleObject.VerticalMoveControl.gameObject.SetActive(false);

        if (BattleObject.BattleTeam == BattleTeam.HeroTeam)
        {
            CameraManager.instance.ClearFollowTarget();
            StageSceneControl.instance.StopStage();
        }
	}
	
	public override void Act()
	{
		base.Act();

		Quaternion rotation = Quaternion.AngleAxis(rotateAngle, Vector3.forward);
		Vector3 diretion = rotation * (_originPos - _hitterPos);
		diretion.y = Mathf.Abs(diretion.y);
		
		float moveSpeed = _flySpeed * Time.deltaTime;
		float rotateSpeed = RotateSpeed * Time.deltaTime;
		float scaleSpeed = ScaleSpeed * Time.deltaTime;
		
		BattleObject.ModelControl.MoveGetHitFly(diretion, moveSpeed, rotateSpeed, scaleSpeed);
	}

    #endregion

	public void AIStartSetting(Vector2 originPos, Vector2 hitterPos, float flySpeed)
	{
		_flySpeed = flySpeed;
		
		_originPos = originPos;
		_hitterPos = hitterPos;
	}
}
