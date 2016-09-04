using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public partial class BlockDataManager
    : DataManager<BlockInfoData, BlockDataManager>, ITitleLoadAsynk
{
	[SerializeField]
	float _selectedBlockId = 0;
	public float SelectedBlockId { get { return _selectedBlockId; } }

    // Method

    #region ITitleLoadAsynk

    public void InitAsynk()
    {
        CreateInstance();
        LoadDataAll();
    }

    #endregion

    public void LoadDataById(float id)
    {
        ClearData();

        _selectedBlockId = id;
        LoadData();
    }

    public void SaveDataById(float id)
    {
        _selectedBlockId = id;
        SaveData();
    }

    #region DataManager

	public override void LoadData()
	{
		ClearData();

        int dValue = (int)(SelectedBlockId);
        string postFixD = dValue.ToString("D4");
        // postFixD Format example : 0001

        float fValue = SelectedBlockId - dValue;
        string postFixF = fValue.ToString("F2");
        postFixF = postFixF.Substring(1);
        // postFixF Format example : .01

        string postFix = string.Format("{0}{1}", postFixD, postFixF);

        LoadData("BlockInfoData" + postFix, DataLoadType.Resources);
	}

	public override void SaveData()
	{
        int dValue = (int)(SelectedBlockId);
        string postFixD = dValue.ToString("D4");
        // postFixD Format example : 0001

        float fValue = SelectedBlockId - dValue;
        string postFixF = fValue.ToString("F2");
        postFixF = postFixF.Substring(1);
        // postFixF Format example : .01

        string postFix = string.Format("{0}{1}", postFixD, postFixF);

        SaveData("BlockInfoData" + postFix, DataLoadType.Resources);
	}

    #endregion

	public BlockInfoData GetLoadedBlockInfoData()
	{
        if (_dataList.Count <= 0)
			return null;

        return _dataList[0];
	}

    public void AddInfoData(BlockInfoData infoData)
    {
        _dataList.Add(infoData);
    }

    public BlockInfoData InfoDataAtBlockId(float blockId)
    {
        return _dataList.Find(
            (BlockInfoData infoData) =>
            {
                return infoData._blockId == blockId;
            });
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(BlockDataManager))]
public class BlockDataManagerEditor 
    : DataManagerEditor<BlockInfoData, BlockDataManager>
{
}

#endif