using UnityEngine;
using System.Collections;

public class ItemGate : Item {
	
	public override string prefab { get { return "ItemGate"; } }

	public int _oepnFieldMainId = 0;
	public int _openFieldSubId = 0;

	public override void SetInfoData(ItemInfoData itemInfo)
	{
		base.SetInfoData(itemInfo);

		foreach (string variable in itemInfo._variableList)
		{
			ReflectionHelper.instance.MemberParse(this, variable);
		}
	}

	public override ItemInfoData CreateItemInfo()
	{
		ItemInfoData tileInfo = base.CreateItemInfo();
		tileInfo._variableList.Add(string.Format("_oepnFieldMainId:{0}", _oepnFieldMainId));
		tileInfo._variableList.Add(string.Format("_openFieldSubId:{0}", _openFieldSubId));
		
		return tileInfo;
	}

	public override void EnterTile(ChaController cha)
	{
		// Fade in
		
		// Field clear and setting
		
		// Character setting
		
		// Fade out
	}
	
	public override void LeaveTile(ChaController cha)
	{
	}

	public override bool IsReserveDestroy()
	{
		return _isReserveDestroy;
	}
}
