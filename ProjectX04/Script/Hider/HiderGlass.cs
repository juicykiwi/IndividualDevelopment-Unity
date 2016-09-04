using UnityEngine;
using System.Collections;

public class HiderGlass : HiderBase {
	
	public override string prefab { get { return "HiderGlass"; } }

	public override void SetInfoData(HiderInfoData info)
	{
		base.SetInfoData(info);
	}
	
	public override HiderInfoData CreateInfoData()
	{
		HiderInfoData info = base.CreateInfoData();
		return info;
	}
}
