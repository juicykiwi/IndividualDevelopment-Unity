using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {

	// Field & Property
	
	public ChaController cha { get; set; }	
	public AniController ani { get; set; }

	public AIBase _curAI = null;

	protected Dictionary<AIState, AIBase> _aiDict = new Dictionary<AIState, AIBase>();

	// Method

	protected virtual void Awake () {

	}

	protected virtual void Start () {

	}

	protected virtual void OnEnable () {
		SetState(AIState.Spawn, null);
	}

	protected virtual void OnDisable () {

	}

	public void SetState(AIState state, AIMessage message)
	{
		if (_aiDict.ContainsKey(state) == false)
			return;

		if (_curAI)
		{
			if (_curAI.IsPossibleNextAIState(state) == false)
			{
				return;
			}

			_curAI.enabled = false;
		}

		_curAI = _aiDict[state];
		_curAI._aiMessage = message;
		_curAI.enabled = true;
	}

	public void RequestMove(Direction direction)
	{
		AIMessageMove message = new AIMessageMove();
		{
			message._direction = direction;
		}
		SetState(AIState.Move, message);
	}

	public void RequestMove(Vector2 movePos)
	{
		AIMessageMove message = new AIMessageMove();
		{
			message._movePos = movePos;
		}
		SetState(AIState.Move, message);
	}

//	public bool IsDetectRange(Vector2 targetPos)
//	{
//		float distance = Vector2.Distance(cha.GetPos(), targetPos);
//		if (distance > cha.stat._detectRange)
//			return false;
//
//		return true;
//	}

	public bool IsAttackRange(Vector2 targetPos)
	{
		float distance = Vector2.Distance(cha.GetPos(), targetPos);
		if (distance > cha.stat._attackRange)
			return false;

		return true;
	}
}
