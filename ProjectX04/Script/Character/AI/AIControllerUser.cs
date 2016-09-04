using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIControllerUser : AIController {
	
	// Method

	protected override void Awake () {
		base.Awake();

		// Add AI
		
		List<System.Type> addAIList = new List<System.Type>();
		{
			addAIList.Add(System.Type.GetType("AISpawn"));
			addAIList.Add(System.Type.GetType("AIIdleForUser"));
			addAIList.Add(System.Type.GetType("AIMoveForUser"));
			addAIList.Add(System.Type.GetType("AIDieForUser"));
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
