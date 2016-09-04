using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StageDataManager 
    : DataManager<StageInfoData, StageDataManager>, ITitleLoadAsynk
{
	[SerializeField]
	int _selectedStageId = 0;
	public int SelectedStageId
	{
		get { return _selectedStageId; }
		set { _selectedStageId = value; }
	}

    public StageInfoData SelectedStageData
    {
        get
        {
            if (_dataList.Count <= 0)
                return null;
            
            return _dataList[0] ; 
        } 
    }

	// Method

    #region ITitleLoadAsynk

    public void InitAsynk()
    {
        CreateInstance();
        LoadDataAll();
    }

    #endregion

    public void LoadDataById(int id)
    {
        _selectedStageId = id;
        LoadData();
    }

    public void SaveDataById(int id)
    {
        _selectedStageId = id;
        SaveData();
    }

    #region DataManager<>

	public override void LoadData()
	{
		ClearData();
		
		string postFix = string.Format("{0:D4}", SelectedStageId);
        LoadData("StageInfoData" + postFix, DataLoadType.Resources);
	}
	
	public override void SaveData()
	{
		string postFix = string.Format("{0:D4}", SelectedStageId);
        SaveData("StageInfoData" + postFix, DataLoadType.Resources);
	}

    #endregion

    public void Add(StageInfoData infoData)
    {
        if (infoData == null)
            return;

        _dataList.Add(infoData);
    }

    public StageInfoData InfoDataAtStageId(int stageId)
    {
        return _dataList.Find((StageInfoData infoData) =>
            {
                return infoData._stageId == stageId;
            });
    }

	#region StageMaker scene related

	public void LoadDataInTool(int stageId)
	{
		ClearDataInTool();

		LoadDataById(stageId);

        if (_dataList.Count <= 0)
		{
			Debug.LogWarning("_stageBlockInfoDataList not exist data. Count : 0");
			return;
		}

        StageInfoData stageBlockInfoData = _dataList[0];
		if (stageBlockInfoData == null)
		{
			Debug.LogWarning("LoadFail. stageBlockInfoData is null.");
			return;
		}

		foreach (CreationBlockInfo creationBlockInfo in stageBlockInfoData._creationBlockInfoList)
		{
			if (creationBlockInfo == null)
			{
				Debug.LogWarning("creationBlockInfo is null.");
				continue;
			}

			if (creationBlockInfo._blockId <= 0)
			{
				Debug.LogWarning("Load block id is zero");
				continue;
			}
		}
	}

	public void SaveDataInTool(int stageId)
	{
		ClearData();

		StageInfoData stageInfoData = new StageInfoData();

		stageInfoData._stageId = stageId;
        _dataList.Add(stageInfoData);
		
		SaveDataById(stageId);
	}

	public void ClearDataInTool()
	{
		ClearData();
	}

	#endregion
}


#if UNITY_EDITOR

[CustomEditor(typeof(StageDataManager))]
public class StageDataManagerEditor 
    : DataManagerEditor<StageInfoData, StageDataManager>
{
}

#endif
