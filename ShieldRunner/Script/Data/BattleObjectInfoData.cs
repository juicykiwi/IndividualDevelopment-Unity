using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CustomXmlSerializerUtil;

/* BattleObjectInfoData */

[Serializable]
[SerializeDataPath("Data/BattleObjectInfoData/")]
public class BattleObjectInfoData
{
	public string _prefabName = "";

	public int _uniqueId = 0;

	public BattleTeam _battleTeam = BattleTeam.None;
	public BattleObjectType _battleObjectType = BattleObjectType.None;
	public BattleAttackType _battleAttackType = BattleAttackType.None;

	public BattleStat _battleStat = new BattleStat();

	public int _baseEquipWeaponId = 0;
	public int _baseEquipHatId = 0;
	public int _baseEquipShieldId = 0;

    public int _rewardGold = 0;
}