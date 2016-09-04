using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum ChaType
{
	None,
	User,
	Enemy,
}

public class ChaController : MonoBehaviour {

	// Field & Property

	const string ChaStatObjName = "Stat";
	protected ChaStat _stat = null;
	public ChaStat stat { get { return _stat; } }

	protected const string AIObjName = "AI";
	protected AIController _aiController = null;
	public AIController aiController { get { return _aiController; } }

	const string ChaPathFindingObjName = "PathFinding";
	protected ChaPathFinding _pathFinding = null;
	public ChaPathFinding pathFinding { get { return _pathFinding; } }

	int _idInStage = 0;
	public int idInStage { get { return _idInStage; } }

	int _modelId = 0;
	public int modelId { get { return _modelId; } }

	protected ChaType _chaType = ChaType.None;
	public ChaType chaType { get { return _chaType; } }

	protected Team _team = Team.None;
	public Team team { get { return _team; } }

	// Method

	protected virtual void Awake () {

		// Add ChaStat.
		_stat = this.GetComponentInChildren<ChaStat>();
		if (_stat == null)
		{
			System.Type[] addComponentType = { System.Type.GetType("ChaStat") };
			GameObject gameObj = new GameObject (ChaStatObjName, addComponentType);
			if (gameObj)
			{
				_stat = gameObj.GetComponent<ChaStat>();
				gameObj.transform.SetParent (this.transform);
			}
		}

		// Add PathFinding
		_pathFinding = this.GetComponentInChildren<ChaPathFinding>();
		if (_pathFinding == null)
		{
			System.Type[] addComponentType = { System.Type.GetType("ChaPathFinding") };
			GameObject gameObj = new GameObject (ChaPathFindingObjName, addComponentType);
			if (gameObj)
			{
				_pathFinding = gameObj.GetComponent<ChaPathFinding>();
				gameObj.transform.SetParent (this.transform);
			}
		}
	}

	// Use this for initialization
	protected virtual void Start () {

	}

	protected virtual void OnDestroy () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init(ChaModelData chaInfo, int idInStage)
	{
		_idInStage = idInStage;
		_modelId = chaInfo._id;
	}

	public void Disable()
	{
		StopAllCoroutines();
	}

	public void Move(Direction direction, Action actionComplete)
	{
		if (direction == Direction.None)
		{
			if (actionComplete != null)
			{
				actionComplete();
			}

			return;
		}

		Vector2 movePos = GetPos(direction);
		
		TileController tileController = FieldManager.instance.GetTileControllerWithPos(movePos);
		if (tileController == null)
		{
			if (actionComplete != null)
			{
				actionComplete();
			}

			return;
		}

		Move(tileController, actionComplete);
	}

	public void Move(Vector2 movePos, Action actionComplete)
	{
		TileController tileController = FieldManager.instance.GetTileControllerWithPos(movePos);
		if (tileController == null)
		{
			if (actionComplete != null)
			{
				actionComplete();
			}

			return;
		}
		
		Move(tileController, actionComplete);
	}

	public void Move(TileController tileController, Action actionComplete)
	{
		if (tileController == null)
		{
			if (actionComplete != null)
			{
				actionComplete();
			}

			return;
		}

		Vector2 tilePos = tileController.GetPos();
		TileController findedTileController = FieldManager.instance.GetTileControllerWithPos(tilePos);
		if (findedTileController == null)
		{
			if (actionComplete != null)
			{
				actionComplete();
			}

			return;
		}

		TileController isMoveTile = FieldManager.instance.IsMoveTile(findedTileController);
		if (isMoveTile == null)
		{
			if (actionComplete != null)
			{
				actionComplete();
			}

			return;
		}

		if (isMoveTile._isUnderCha == true)
		{
			if (actionComplete != null)
			{
				actionComplete();
			}

			return;
		}

		Move(isMoveTile.transform.position, actionComplete);
	}

	public void Move(Vector3 destPos, Action actionComplete)
	{
		StopCoroutine("CoroutineMove");

		MoveValue moveValue = new MoveValue() {
			_destPos = GameHelper.RoundPos(destPos),
			_actionComplete = actionComplete };

		StartCoroutine(CoroutineMove(moveValue));
	}

	public void MoveWarp(Vector2 warpPos)
	{
		MoveForce(warpPos);
	}

	public void MoveForce(Vector3 destPos)
	{
		StopCoroutine("CoroutineMove");

		Vector2 originPos = GetPos();
		transform.position = GameHelper.RoundPos(destPos);

		MoveStart(originPos, GetPos());
		MoveEnd(originPos, GetPos());
	}

	public struct MoveValue
	{
		public Vector3 _destPos;
		public Action _actionComplete;
	}

	IEnumerator CoroutineMove(MoveValue moveValue)
	{
		Vector2 originPos = GetPos();
		Vector3 direction = Vector3.Normalize(moveValue._destPos - transform.position);

		MoveStart(originPos, (Vector2)moveValue._destPos);

		while (true)
		{
			float distance = Vector3.Distance(transform.position, moveValue._destPos);
			float moveLenght = stat._moveSpeed * Time.deltaTime;
			moveLenght = Mathf.Min(moveLenght, distance);

			transform.Translate(direction * moveLenght);

			if (transform.position == moveValue._destPos)
				break;

			yield return null;
		}

		if (moveValue._actionComplete != null)
		{
			moveValue._actionComplete();
		}

		MoveEnd(originPos, GetPos());
	}

	public virtual void MoveStart(Vector2 originPos, Vector2 destPos)
	{
		if (EventManager.instance._actionChaMoveStart != null)
		{
			EventManager.instance._actionChaMoveStart(aiController.cha, originPos, destPos);
		}
	}

	public virtual void MoveEnd(Vector2 originPos, Vector2 destPos)
	{
		if (EventManager.instance._actionChaMoveFinish != null)
		{
			EventManager.instance._actionChaMoveFinish(aiController.cha, originPos, destPos);
		}
	}

	public Vector2 GetPos()
	{
		return GetPos(Direction.None);
	}

	public Vector2 GetPos(Direction direction)
	{
		Vector2 pos = GameHelper.RoundPos(transform.position);
		return pos + GameHelper.GetVectorWithDirection(direction);
	}

	public virtual void GetHit(ChaController attacker)
	{
		if (stat.IsDie() == true)
			return;

		stat._life -= 1;
	}

	public virtual void RecoveryHp(int value)
	{
		if (stat.IsDie() == true)
			return;

		if (stat.IsFullCurrentHp() == true)
			return;

		stat._life += value;
	}
}
