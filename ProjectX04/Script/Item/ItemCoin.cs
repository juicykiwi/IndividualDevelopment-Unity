using UnityEngine;
using System.Collections;

public class ItemCoin : Item {
	
	public override string prefab { get { return "ItemCoin"; } }

	public override void EnterTile(ChaController cha)
	{
		if (cha.chaType != ChaType.User)
			return;
		
		UserInfoManager.instance.AddCoin(1);
		_isReserveDestroy = true;
	}
	
	public override void LeaveTile(ChaController cha)
	{
	}

	public override bool IsReserveDestroy()
	{
		return _isReserveDestroy;
	}
}
