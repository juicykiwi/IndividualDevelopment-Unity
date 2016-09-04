using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public partial class EquipItemDataManager 
    : DataManager<EquipItemInfoData, EquipItemDataManager>, ITitleLoadAsynk
{
	// Prefab
	PrefabDataDict<EquipItem> _equipItemPrefabDataDict = new PrefabDataDict<EquipItem>();

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

		// xml serialize 
        LoadData("EquipItemInfoData", DataLoadType.Resources);

		// prefab
		_equipItemPrefabDataDict.LoadPrefabAll();
	}

	public override void SaveData()
	{
        SaveData("EquipItemInfoData", DataLoadType.Resources);
	}
	
	public override void ClearData()
	{
        base.ClearData();

		_equipItemPrefabDataDict.Clear();
	}

    #endregion

    public EquipItemInfoData EquipItemAtUniqueId(int uniqueId)
    {
        return _dataList.Find((EquipItemInfoData infoData) =>
            {
                return infoData._uniqueId == uniqueId;
            });
    }

    public GameObject EquipItemPrefabAtName(string name)
    {
        return _equipItemPrefabDataDict.GetPrefabByName(name);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(EquipItemDataManager))]
public class EquipItemDataManagerEditor 
    : DataManagerEditor<EquipItemInfoData, EquipItemDataManager>
{
}

#endif
