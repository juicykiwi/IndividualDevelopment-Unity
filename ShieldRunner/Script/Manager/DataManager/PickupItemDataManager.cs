using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PickupItemDataManager 
    : DataManager<PickupItemInfoData, PickupItemDataManager>, ITitleLoadAsynk
{
    PrefabDataDict<PickupItemObject> _prefabDict = new PrefabDataDict<PickupItemObject>();

    // Method

    #region ITitleLoadAsynk

    public void InitAsynk()
    {
        CreateInstance();
        Init();
    }

    #endregion

    #region DataManager

    public override void LoadData()
    {
        ClearData();

        // xml serialize 
        LoadData("PickupItemInfoData", DataLoadType.Resources);

        // prefab
        _prefabDict.LoadPrefabAll();
    }

    public override void SaveData()
    {
        SaveData("PickupItemInfoData", DataLoadType.Resources);
    }

    public override void ClearData()
    {
        base.ClearData();

        _prefabDict.Clear();
    }

    #endregion

    public PickupItemInfoData InfoDataAtUniqueId(int uniqueId)
    {
        return _dataList.Find((PickupItemInfoData infoData) =>
            {
                return infoData._uniqueId == uniqueId;
            });
    }

    public GameObject PrefabDataAtName(string name)
    {
        return _prefabDict.GetPrefabByName(name);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(PickupItemDataManager))]
public class PickupItemDataManagerEditor 
    : DataManagerEditor<PickupItemInfoData, PickupItemDataManager>
{
}

#endif