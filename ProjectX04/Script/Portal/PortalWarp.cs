using UnityEngine;
using System.Collections;

public class PortalWarp : PortalBase {

	public int _warpField = 0;
	public Vector2 _warpPos = Vector2.zero;

	public override string _prefab { get { return "PortalWarp"; } }
	
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
		info._variableList.Add(string.Format("_warpField:{0}", _warpField));
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

		if (cha.chaType != ChaType.User)
			return;

		if (_warpField == 0)
		{
			cha.MoveWarp(_warpPos);
			return;
		}

		StageManager.instance.WarpField(_warpField, _warpPos);
	}
	
	public override void LeaveTile (ChaController cha)
	{
		
	}
}
