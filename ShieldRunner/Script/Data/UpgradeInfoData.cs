using UnityEngine;
using System;
using System.Collections;
using CustomXmlSerializerUtil;

[Serializable]
[SerializeDataPath("Data/UpgradeInfoData/")]
public class UpgradeInfoData
{
    public EquipItemMainType _mainType = EquipItemMainType.None;

    public int _grade = 0;

    public int _cost = 0;

    public int _targetEquipItemId = 0;
    public int _upgradeEquipItemId = 0;
}