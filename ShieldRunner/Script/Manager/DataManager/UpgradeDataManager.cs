using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UpgradeDataManager 
    : DataManager<UpgradeInfoData, UpgradeDataManager>, ITitleLoadAsynk
{
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
        LoadData("UpgradeInfoData", DataLoadType.Resources);
    }

    public override void SaveData()
    {
        SaveData("UpgradeInfoData", DataLoadType.Resources);
    }

    #endregion

    public UpgradeInfoData InfoDataAtEquipItemId(int equipItemId)
    {
        return _dataList.Find((UpgradeInfoData infoData) =>
            {
                return infoData._targetEquipItemId == equipItemId;
            });
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(UpgradeDataManager))]
public class UpgradeDataManagerEditor 
    : DataManagerEditor<UpgradeInfoData, UpgradeDataManager>
{
}

#endif