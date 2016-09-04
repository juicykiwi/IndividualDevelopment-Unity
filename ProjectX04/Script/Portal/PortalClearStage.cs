using UnityEngine;
using System.Collections;

public class PortalClearStage : PortalBase {

	public override string _prefab { get { return "PortalClearStage"; } }

	public override void EnterTile (ChaController cha)
	{
	}

	public override void EnterCompleteTile (ChaController cha)
	{
		if (cha == null)
			return;
		
		if (cha.chaType != ChaType.User)
			return;
		
		if (EventManager.instance != null)
		{
			EventManager.instance._actionSuccessMission();
		}
	}
	
	public override void LeaveTile (ChaController cha)
	{
	}
}
