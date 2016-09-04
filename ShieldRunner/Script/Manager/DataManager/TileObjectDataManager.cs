using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TileObjectDataManager 
    : DataManager<TileObjectInfoData, TileObjectDataManager>, ITitleLoadAsynk
{
	// Prefab
	public PrefabDataDict<TileObject> _tileObjectPrefabDataDict =
		new PrefabDataDict<TileObject>();

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
        LoadData("TileObjectInfoData", DataLoadType.Resources);
		
		// prefab
		_tileObjectPrefabDataDict.LoadPrefabAll();
	}
	
	public override void SaveData()
	{
        SaveData("TileObjectInfoData", DataLoadType.Resources);
	}
	
	public override void ClearData()
	{
        base.ClearData();

		_tileObjectPrefabDataDict.Clear();
	}

    #endregion

    public TileObjectInfoData InfoDataAtUniqueId(int uniqueId)
    {
        return _dataList.Find((TileObjectInfoData infoData) =>
            {
                return infoData._uniqueId == uniqueId;
            });
    }

    public GameObject PrefabDataAtName(string name)
    {
        return _tileObjectPrefabDataDict.GetPrefabByName(name);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(TileObjectDataManager))]
public class TileObjectDataManagerEditor
    : DataManagerEditor<TileObjectInfoData, TileObjectDataManager>
{
}

#endif