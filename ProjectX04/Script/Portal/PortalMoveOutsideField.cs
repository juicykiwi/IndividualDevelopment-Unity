using UnityEngine;
using System.Collections;

public class PortalMoveOutsideField : PortalBase {

	public Vector2 _warpPos = Vector2.zero;
	
	public override string _prefab { get { return "PortalMoveOutsideField"; } }
	
	public override void SetInfoData(PortalInfoData info)
	{
		base.SetInfoData(info);
		
		foreach (string variable in info._variableList)
		{
			ReflectionHelper.instance.MemberParse(this, variable);
		}
	}
	
	public override PortalInfoData CreateItemInfo()
	{
		PortalInfoData info = base.CreateItemInfo();
		info._variableList.Add(string.Format("_warpPos:{0}", _warpPos));
		
		return info;
	}
	
	public override void EnterTile (ChaController cha)
	{
		
	}
	
	public override void EnterCompleteTile (ChaController cha)
	{
		if (cha == null)
			return;
		
		switch (cha.chaType)
		{
		case ChaType.User:
		{
			cha.MoveWarp(_warpPos);
		}
			break;
			
		case ChaType.Enemy:
		default:
			break;
		}
	}
	
	public override void LeaveTile (ChaController cha)
	{
		
	}
}
