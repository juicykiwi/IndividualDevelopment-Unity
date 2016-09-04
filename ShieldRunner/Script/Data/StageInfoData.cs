using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CustomXmlSerializerUtil;

[Serializable]
public class CreationBlockInfo
{
	public float _blockId = 0;

    // 반복 확률
	public int _frequencyValue = 0;
}

// StageBlockInfoData

[Serializable]
[SerializeDataPath("Data/StageInfoData/")]
public class StageInfoData
{
	public int _stageId = 0;

    // 랜덤으로 블럭 선택하여 만드는지 여부
    public bool _isRandomCreation = false;

    // 현재 스테이지에서 블럭 만드는 횟수
    public int _createCount = 0;

    // 스테이지 변경 시 준비하는 블럭의 아이디
	public float _readyBlockId = 0;
    // 스테이지 변경 시 준비하는 블럭의 길이
	public float _readyBlockLength = 0f;

    // 스테이지 클리어 시 보상 받는 골드
    public int _clearRewardGold = 0;

	public List<CreationBlockInfo> _creationBlockInfoList = new List<CreationBlockInfo>();
}