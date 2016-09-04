using UnityEngine;
using System;
using System.Collections;

public class KeyManager : ManagerBase<KeyManager> {

	public Action<Direction> _keyDirectionInputAction;

	// Method

	protected override void Awake()
	{
		base.Awake ();
	}

	void Update()
	{
		if (_keyDirectionInputAction == null)
			return;

		if (Input.GetKeyDown(KeyCode.RightArrow) == true)
		{
			_keyDirectionInputAction(Direction.Right);
		}

		else if (Input.GetKeyDown(KeyCode.LeftArrow) == true)
		{
			_keyDirectionInputAction(Direction.Left);
		}

		else if (Input.GetKeyDown(KeyCode.UpArrow) == true)
		{
			_keyDirectionInputAction(Direction.Up);
		}

		else if (Input.GetKeyDown(KeyCode.DownArrow) == true)
		{
			_keyDirectionInputAction(Direction.Down);
		}
	}
}
