using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageControl : MonoBehaviour
{
	[SerializeField]
	StageBlockControl _blockControl = null;

	[SerializeField]
	SortedList _stageObjectSortedList = new SortedList();

	[SerializeField]
	int _currentStageIndex = 0;

	[SerializeField]
	float _stageRunTime = 0f;

    // Method

	#region MonoBehavior event

	public void FixedUpdate()
	{
		_stageRunTime += Time.fixedDeltaTime;

		if (CheckStageMissionSuccess() == true)
		{
			ChangeNextStage();
		}
	}

#if UNITY_EDITOR

	void OnGUI()
	{
        int stageId = CurrentStageObject().InfoData._stageId;
        GUIStyle style = new GUIStyle();
        style.fontSize = 10;
        string Level = "<color=red>Level : " +  stageId.ToString() + "</color>";
        GUI.Label(new Rect(0f, 70f, 0f, 10f), Level, style);
	}

#endif

	#endregion

	#region Init / Clear

	public void Init()
	{
		Clear();

		if (_blockControl == null)
		{
			_blockControl = GameObjectHelper.NewGameObject<StageBlockControl>();
			_blockControl.transform.SetParent(transform);
		}

        IEnumerator<StageInfoData> stageEnumerator = StageDataManager.instance.DataListEnumerator;
        while (stageEnumerator.MoveNext() == true)
        {
            StageInfoData infoData = stageEnumerator.Current;
            if (infoData == null)
                continue;

            if (_stageObjectSortedList.ContainsKey(infoData._stageId) == true)
            {
                Debug.LogWarning("Already contain StageObject key. stage id : " + infoData._stageId.ToString());
                continue;
            }

            StageObject stageObject = StageObject.CreateStageObject(infoData);
            stageObject.transform.SetParent(transform);

            _stageObjectSortedList.Add(infoData._stageId, stageObject);
        }
	}

	public void Clear()
	{
		_currentStageIndex = 0;
		_stageObjectSortedList.Clear();
	}

	#endregion

	#region Start / Stop

	public void StartStage()
	{
		if (CurrentStageObject() == null)
			return;

		_blockControl.StartBlockCreator(CurrentStageObject().InfoData);
	}

	public void StopStage()
	{
		_blockControl.StopBlockCreator();
	}

	#endregion

	public void OnEndStage(int stageId)
	{
		ChangeNextStage();
	}

	bool CheckStageMissionSuccess()
	{
		if (CurrentStageObject() == null)
			return false;

        if (_blockControl.CreatedCount < CurrentStageObject().InfoData._createCount)
            return false;

        return true;
	}

	void ChangeNextStage()
	{
        StageObject stageObject = CurrentStageObject();
        if (stageObject == null)
            return;

		StageObject nextStageObject = NextStageObject();
		if (nextStageObject == null)
			return;

        int rewardGold = stageObject.InfoData._clearRewardGold;
        PlayerDataManager.instance.IncreaseGold(rewardGold);

		_stageRunTime = 0f;
		_blockControl.StopBlockCreator();

		++_currentStageIndex;

		_blockControl.StartBlockCreator(CurrentStageObject().InfoData);
	}

	StageObject CurrentStageObject()
	{
		if (_stageObjectSortedList.Count <= _currentStageIndex)
			return null;
		
		return _stageObjectSortedList.GetByIndex(_currentStageIndex) as StageObject;
	}

	StageObject NextStageObject()
	{
		int nextStageIndex = _currentStageIndex + 1;

		if (_stageObjectSortedList.Count <= nextStageIndex)
			return null;

		return _stageObjectSortedList.GetByIndex(nextStageIndex) as StageObject;
	}
}
