using UnityEngine;
using System;
using System.Collections;

public class UIManager : ManagerBase<UIManager> {

	public Action<int> _changedCoinCountAction = null;
	public Action<ChaController> _changedUserHpAction = null;

	// Method

	protected override void Awake()
	{
		base.Awake();
	}
}
