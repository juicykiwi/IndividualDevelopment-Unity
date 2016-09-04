using UnityEngine;
using System;
using System.Collections;
using CustomXmlSerializerUtil;

/* EquipItemInfoData */
[Serializable]
[SerializeDataPath("Data/EquipItemInfoData/")]
public class EquipItemInfoData
{
	public string _prefabName = "";

	public int _uniqueId = 0;

	public EquipItemMainType _equipItemMainType = EquipItemMainType.None;

	public BattleStatType _battleStatType = BattleStatType.None;
	public int _battleStatValue = 0;

    // 내구력
    public bool _useDurability = false;
    public int _durabilityValue = 0;

    // Method

    public EquipItemInfoData Clone()
    {
        EquipItemInfoData clone = new EquipItemInfoData()
            {
                _prefabName = this._prefabName,
                _uniqueId = this._uniqueId,
                _equipItemMainType = this._equipItemMainType,
                _battleStatType = this._battleStatType,
                _battleStatValue = this._battleStatValue,
                _useDurability = this._useDurability,
                _durabilityValue = this._durabilityValue,
            };

        return clone;
    }
}
