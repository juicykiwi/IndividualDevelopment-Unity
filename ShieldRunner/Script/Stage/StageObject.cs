using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class StageObject : MonoBehaviour
{
	[SerializeField]
	StageInfoData _infoData = null;
	public StageInfoData InfoData
	{
		get { return _infoData; }
		set { _infoData = value; }
	}

    // Method

    #region Create, Destroy

	public static StageObject CreateEmptyStageObject()
	{
		StageInfoData infoData = new StageInfoData();
		return CreateStageObject(infoData);
	}

	public static StageObject CreateStageObject(int stageId)
	{
        StageInfoData infoData = StageDataManager.instance.InfoDataAtStageId(stageId);
		return CreateStageObject(infoData);
	}

	public static StageObject CreateStageObject(StageInfoData infoData)
	{
		if (infoData == null)
		{
			Debug.LogError("Fail create StageObject. infoData is null.");
			return null;
		}

		StageObject stageObject = GameObjectHelper.NewGameObject<StageObject>();
		if (stageObject == null)
			return null;

		stageObject.InfoData = infoData;

		return stageObject;
	}
	
	public void RemoveStageObject()
	{
		Destroy(gameObject);
	}
        
    #endregion
}



