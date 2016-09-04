using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIControllerEnemy : AIController {

	// Method

	protected override void Awake () {
		base.Awake();
		
		// Add AI
		
		List<System.Type> addAIList = new List<System.Type>();
		{
			addAIList.Add(System.Type.GetType("AISpawn"));
			addAIList.Add(System.Type.GetType("AIIdle"));
			addAIList.Add(System.Type.GetType("AIDetect"));
			addAIList.Add(System.Type.GetType("AIAttack"));
			addAIList.Add(System.Type.GetType("AIMove"));
			addAIList.Add(System.Type.GetType("AIDie"));
		}
		
		foreach (System.Type type in addAIList)
		{
			AIBase ai = this.gameObject.AddComponent(type) as AIBase;
			if (ai == null)
				continue;
			
			ai.aiController = this;
			_aiDict.Add(ai.state, ai);
		}
	}

	protected override void Start () {

	}
}
