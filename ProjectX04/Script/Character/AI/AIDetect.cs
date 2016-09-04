using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIDetect : AIBase {
	
	// Method

	protected override void Awake ()
	{
		base.Awake ();

		_state = AIState.Detect;
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate ();

		ChaPathFinding pathFinding = _aiController.cha.pathFinding;

		ChaController detectedCha = DetectEnemy();

		if (detectedCha == null)
		{
			aiController.SetState(AIState.Idle, null);
			return;
		}

		if (IsLookTarget(detectedCha) == false)
		{
			aiController.SetState(AIState.Idle, null);
			return;
		}

		ChaController userCHa = ChaManager.instance.GetUserCha();
		if (userCHa == null)
		{
			aiController.SetState(AIState.Idle, null);
			return;
		}

		if (pathFinding)
		{
			pathFinding.FindPath(_aiController.cha.GetPos(),
			                     userCHa.GetPos(),
			                     FindPathSuccess,
			                     FindPathFail);
		}
	}

	bool IsLookTarget(ChaController target)
	{
		if (target == null)
			return false;

		Vector2 chaPos = aiController.cha.GetPos();
		Vector2 targetPos = target.GetPos();
		Vector2 toTargetVector = targetPos - chaPos;

		RaycastHit2D[] hits = Physics2D.RaycastAll(
			chaPos, toTargetVector.normalized, toTargetVector.magnitude);

		if (hits.Length <= 0)
			return true;

		float lookThroughRatio = 0f;

		foreach (RaycastHit2D hit in hits)
		{
			if (hit.collider.gameObject == null)
				continue;

			HiderBase hider = hit.collider.gameObject.GetComponent<HiderBase>();
			if (hider == null)
				continue;

			if (GameHelper.RoundPos(hider.transform.position) == chaPos)
				continue;

			lookThroughRatio += hider._lookThroughRatio;
			if (lookThroughRatio >= 100f)
				return false;
		}

		EffectManager.instance.PlayEffect(
			"DettectedUserEffect", aiController.cha.gameObject, new Vector2(-0.25f, 0.25f));

		return true;
	}

	ChaController DetectEnemy()
	{
		ChaController detectedCha = null;

		Vector2 pos = aiController.cha.GetPos();
		int detectRange = aiController.cha.stat._detectRange;

		int detectPosYMin = (int)pos.y - detectRange;
		int detectPosYMax = (int)pos.y + detectRange;

		for (int y = detectPosYMin; y <= detectPosYMax; ++y)
		{
			int rangeX = detectRange - Mathf.Abs(y - (int)pos.y);

			int detectPosXMin = (int)pos.x - rangeX;
			int detectPosXMax = (int)pos.x + rangeX;

			for (int x = detectPosXMin; x <= detectPosXMax; ++x)
			{
				if (StageManager.instance.CheckPosRayHit(
					new Vector2(x, y), CheckRayHitType.UserCha) == false)
				{
					continue;
				}

				detectedCha = ChaManager.instance.GetUserCha();
				break;
			}

			if (detectedCha)
			{
				break;
			}
		}

		return detectedCha;
	}

	void FindPathSuccess(List<Vector3> pathList)
	{
		if (pathList.Count <= 1)
		{
			aiController.SetState(AIState.Idle, null);
			return;
		}

		if (pathList.Count <= 2)
		{
			aiController.SetState(AIState.Attack, null);
			return;
		}

		AIMessageMove message = new AIMessageMove();
		{
			message._direction = GameHelper.GetDirectionWithPos(
				aiController.cha.GetPos(),
				new Vector2(pathList[1].x, pathList[1].y));
		}
		aiController.SetState(AIState.Move, message);
	}

	void FindPathFail()
	{
		aiController.SetState(AIState.Idle, null);
	}
}
