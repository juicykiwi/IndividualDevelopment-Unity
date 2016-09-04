using UnityEngine;
using System.Collections;

public class ItemRecoveryHp : Item {

	public override string prefab { get { return "ItemRecoveryHp"; } }

	public override void EnterTile(ChaController cha)
	{
		if (cha.chaType != ChaType.User)
			return;

		ChaController userCha = ChaManager.instance.GetUserCha();
		if (userCha == null)
			return;

		userCha.RecoveryHp(1);
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
