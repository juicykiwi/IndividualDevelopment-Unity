using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BattleObjectDataManager
    : DataManager<BattleObjectInfoData, BattleObjectDataManager>, ITitleLoadAsynk
{
    List<BattleObjectInfoData> _heroDataList = new List<BattleObjectInfoData>();
    List<BattleObjectInfoData> _monsterDataList = new List<BattleObjectInfoData>();

	// Prefab
	public PrefabDataDict<BattleObject> _battleObjectPrefabDataDict = new PrefabDataDict<BattleObject>();

	// Method

    #region ITitleLoadAsynk

    public void InitAsynk()
    {
        CreateInstance();
        Init();
    }

    #endregion

    #region DataManager<>

	public override void LoadData()
	{
		ClearData();

        LoadData("BattleObjectInfoData", DataLoadType.Resources);
        AsortDataByType();

		// prefab
		_battleObjectPrefabDataDict.LoadPrefabAll();
	}

	public override void SaveData()
	{
        SaveData("BattleObjectInfoData", DataLoadType.Resources);
	}
	
	public override void ClearData()
	{
        base.ClearData();

		_battleObjectPrefabDataDict.Clear();
	}

    #endregion

    void AsortDataByType()
    {
        _heroDataList.Clear();
        _monsterDataList.Clear();

        for (int index = 0; index < _dataList.Count; ++index)
        {
            if (_dataList[index] == null)
                continue;

            switch (_dataList[index]._battleObjectType)
            {
                case BattleObjectType.Hero:
                    _heroDataList.Add(_dataList[index]);
                    break;

                case BattleObjectType.Monster:
                case BattleObjectType.BossMonster:
                    _monsterDataList.Add(_dataList[index]);
                    break;

                default:
                    break;
            }
        }
    }

    public GameObject PrefabDataAtName(string name)
    {
        return _battleObjectPrefabDataDict.GetPrefabByName(name);
    }

    public BattleObjectInfoData InfoDataAtUniqueId(int uniqueId)
    {
        return _dataList.Find(
            (BattleObjectInfoData infoData) =>
            {
                return infoData._uniqueId == uniqueId;
            });
    }

    public List<BattleObjectInfoData> GetBattleInfoDataList(BattleObjectType type)
    {
        switch (type)
        {
            case BattleObjectType.Hero:
                return _heroDataList;

            case BattleObjectType.Monster:
            case BattleObjectType.BossMonster:
                return _monsterDataList;

            default:
                break;
        }

        return new List<BattleObjectInfoData>();
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(BattleObjectDataManager))]
public class BattleObjectDataManagerEditor
    : DataManagerEditor<BattleObjectInfoData, BattleObjectDataManager>
{
}

#endif
