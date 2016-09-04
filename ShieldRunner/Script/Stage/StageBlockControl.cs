using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StageBlockControl : MonoBehaviour
{
	[SerializeField]
	float _offsetLenght = 5f;

	StageInfoData _stageInfoData = null;
	
	bool _isStartCreateCoroutine = true;

    int _createdCount = 0;
    public int CreatedCount { get { return _createdCount; } }

	int _frequensyValueTotal = 0;
	List<CreationBlockInfo> _creationBlockInfoList = new List<CreationBlockInfo>();

	float _lastCreatedBlockPos = 0f;
	public float LastCreatedBlockPos { get { return _lastCreatedBlockPos; } }

    // Delay mode

    bool _isRunningAsyncMode = false;

    // Method

	#region MonoBehaviour event

	void OnDestroy()
	{
		StopBlockCreator();
	}

	#endregion

	#region Start / Stop

	public void StartBlockCreator(StageInfoData stageInfoData)
	{
		StopBlockCreator();
		
		_stageInfoData = stageInfoData;
		if (_stageInfoData == null)
			return;
		
		SettingCreationBlock(stageInfoData._creationBlockInfoList);
		
		if (_isStartCreateCoroutine == true)
		{
			Vector3 screenMinPoint = CameraManager.instance.GetScreenToWorldPoint(Vector2.zero);
			_lastCreatedBlockPos = screenMinPoint.x - _offsetLenght;
			
			CreateReadyBlock(stageInfoData._readyBlockId, stageInfoData._readyBlockLength);
			
			_isStartCreateCoroutine = false;
		}
		
        // 스테이지 생성을 비동기 모드로 변경
		//StartCoroutine(CreatorLoopCoroutine_ImmediatelyMode());

        StartCoroutine(CreatorLoopCoroutine_AsyncMode());
	}

	void SettingCreationBlock(List<CreationBlockInfo> creationBlockInfoList)
	{
        _createdCount = 0;
		_frequensyValueTotal = 0;
		_creationBlockInfoList.Clear();
		
		_creationBlockInfoList.AddRange(creationBlockInfoList);
		
		foreach (CreationBlockInfo creationBlockInfo in _creationBlockInfoList)
		{
			_frequensyValueTotal += creationBlockInfo._frequencyValue;
		}
	}
	
	public void StopBlockCreator()
	{
        // 스테이지 생성을 비동기 모드로 변경
        // StopCoroutine(CreatorLoopCoroutine_ImmediatelyMode());

        StopCoroutine(CreatorLoopCoroutine_AsyncMode());
	}

	#endregion

	#region Create helper

    IEnumerator CreatorLoopCoroutine_ImmediatelyMode()
    {
        while (true)
        {
            if (IsNeedCreateBlock() == false)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            if (_stageInfoData._isRandomCreation == true)
            {
                float createBlockId = GetRandomCreationBlockId();
                CreateBlock(createBlockId);
            }
            else
            {
                float createId = GetCurrentCreationBlockId();
                CreateBlock(createId);
            }

            _createdCount += 1;

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator CreatorLoopCoroutine_AsyncMode()
    {
        while (true)
        {
            if (_isRunningAsyncMode == true)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            if (IsNeedCreateBlock() == false)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            _isRunningAsyncMode = true;

            if (_stageInfoData._isRandomCreation == true)
            {
                float randomId = GetRandomCreationBlockId();
                CreateBlockAsync(randomId, OnCompleteSettingAsync);
            }
            else
            {
                float createId = GetCurrentCreationBlockId();
                CreateBlockAsync(createId, OnCompleteSettingAsync);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    float GetRandomCreationBlockId()
    {
        if (_creationBlockInfoList.Count <= 0)
            return 0;

        int randomValue = UnityEngine.Random.Range(0, _frequensyValueTotal);

        for (int index = 0; index < _creationBlockInfoList.Count; ++index)
        {
            CreationBlockInfo creationBlockInfo = _creationBlockInfoList[index];
            if (creationBlockInfo == null)
                continue;

            randomValue -= creationBlockInfo._frequencyValue;
            if (randomValue <= 0)
                return creationBlockInfo._blockId;
        }

        return _creationBlockInfoList[0]._blockId;
    }

    float GetCurrentCreationBlockId()
    {
        if (_creationBlockInfoList.Count <= 0)
            return 0;

        int index = (_createdCount % _creationBlockInfoList.Count);
        return _creationBlockInfoList[index]._blockId;
    }

    bool IsNeedCreateBlock()
    {
        Vector2 screenSize = ScreenManager.instance.ScreenSize;
        Vector3 screenMaxPoint = CameraManager.instance.GetScreenToWorldPoint(screenSize);

        return (_lastCreatedBlockPos <= screenMaxPoint.x + _offsetLenght);
    }

    void OnCompleteSettingAsync(float blockBoundWidth)
    {
        _isRunningAsyncMode = false;
        _lastCreatedBlockPos += blockBoundWidth;
        _createdCount += 1;
    }
        
    #endregion

    #region Create async mode

    void CreateBlockAsync(float blockId, Action<float> completeAction)
    {
        BlockObject blockObject = BlockObject.CreateBlockObject(blockId, new Vector2(_lastCreatedBlockPos, 0f), true);
        if (blockObject == null)
        {
            _lastCreatedBlockPos += 1f;
            return;
        }

        blockObject.StartSettingAsyncType();
        blockObject.CompleteSettingAsyncEvent += completeAction;
    }

    #endregion

    #region Create immediately mode
	
	void CreateReadyBlock(float readyBlockId, float readyBlockLength)
	{
		float beginCreatedBlockPos = _lastCreatedBlockPos;
		
		do
		{
			bool isOverLength = (readyBlockLength < (_lastCreatedBlockPos - beginCreatedBlockPos));
			if (isOverLength == true)
				break;
			
			CreateBlock(readyBlockId);
			
		} while(IsNeedCreateBlock() == true);
	}
	
	void CreateBlock(float blockId)
	{
		BlockObject blockObject = BlockObject.CreateBlockObject(blockId, new Vector2(_lastCreatedBlockPos, 0f));
		if (blockObject == null)
		{
			_lastCreatedBlockPos += 1f;
			return;
		}

		_lastCreatedBlockPos += blockObject.BountRect.width;
	}

	#endregion
}
