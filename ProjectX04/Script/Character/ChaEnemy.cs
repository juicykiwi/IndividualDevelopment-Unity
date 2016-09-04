using UnityEngine;
using System.Collections;

public class ChaEnemy : ChaController {

	// Field & Property

	// Method
	
	protected override void Awake () {
		base.Awake();

		_chaType = ChaType.Enemy;
		_team = Team.EnemyAITeam;

		// Add AI
		_aiController = this.GetComponentInChildren<AIControllerEnemy>();
		if (_aiController == null)
		{
			System.Type[] addComponentType = { System.Type.GetType("AIControllerEnemy") };
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
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
	}
}
