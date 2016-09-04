using UnityEngine;
using System.Collections;

public class ChaUser : ChaController {

	// Property

	// Method

	protected override void Awake () {
		base.Awake();

		_chaType = ChaType.User;
		_team = Team.UserTeam;

		// Add AI
		_aiController = this.GetComponentInChildren<AIControllerUser>();
		if (_aiController == null)
		{
			System.Type[] addComponentType = { System.Type.GetType("AIControllerUser") };
			GameObject gameObj = new GameObject (AIObjName, addComponentType);
			if (gameObj)
			{
				_aiController = gameObj.GetComponent<AIController>();
				gameObj.transform.SetParent (this.transform);
			}
		}
		_aiController.cha = this;
		_aiController.ani = this.GetComponent<AniController>();
	}

	protected override void Start ()
	{
		base.Start ();

		if (EventManager.instance != null)
		{
			EventManager.instance._actionTouchUserCha += OnTouchUserCha;
			EventManager.instance._actionCancelTouchUserCha += OnCancelTouchUserCha;
		}
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();

		if (EventManager.instance)
		{
			EventManager.instance._actionTouchUserCha -= OnTouchUserCha;
			EventManager.instance._actionCancelTouchUserCha -= OnCancelTouchUserCha;
		}
	}

	public void OnTouchUserCha()
	{
	}

	public void OnCancelTouchUserCha()
	{
	}

	public override void MoveStart(Vector2 originPos, Vector2 destPos)
	{
		base.MoveStart(originPos, destPos);
	}
	
	public override void MoveEnd(Vector2 originPos, Vector2 destPos)
	{
		base.MoveEnd(originPos, destPos);
	}

	public override void GetHit(ChaController attacker)
	{
		base.GetHit(attacker);

		if (UIManager.instance._changedUserHpAction != null)
		{
			UIManager.instance._changedUserHpAction(this);
		}

		if (_stat.IsDie() == true)
		{
			aiController.SetState(AIState.Die, null);
		}
	}

	public override void RecoveryHp(int value)
	{
		base.RecoveryHp (value);

		if (UIManager.instance._changedUserHpAction != null)
		{
			UIManager.instance._changedUserHpAction(this);
		}
	}
}
