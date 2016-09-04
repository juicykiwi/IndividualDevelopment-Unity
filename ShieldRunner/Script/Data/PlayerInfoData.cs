using UnityEngine;
using System;
using System.Collections;
using CustomXmlSerializerUtil;

[Serializable]
[SerializeDataPath("")]
public class PlayerInfoData
{
	public float _bestMoveDistance = 0f;

    public int _haveGold = 0;

    public int _lastEquipedShieldId = 0;

    public int _enchatCount = 0;
}