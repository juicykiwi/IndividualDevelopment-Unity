using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CustomXmlSerializerUtil;

// CreationTileInfo

[Serializable]
public class CreationTileInfo
{
	public int _uniqueId = 0;
	public Vector2 _pos = Vector2.zero;

    // Method

	public void SetData(TileObject tileObject)
	{
		_uniqueId = tileObject.InfoData._uniqueId;
		_pos = tileObject.transform.position;
	}
}

// CreationBattleObjectInfo

[Serializable]
public class CreationBattleObjectInfo
{
	public int _uniqueId = 0;
	public Vector2 _pos = Vector2.zero;

    // Method

	public void SetData(MonsterObject monsterObject)
	{
		_uniqueId = monsterObject.InfoData._uniqueId;
		_pos = monsterObject.transform.position;
	}
}

// CreationPickupItemObjectInfo

[Serializable]
public class CreationPickupItemObjectInfo
{
	public int _uniqueId = 0;
	public Vector2 _pos = Vector2.zero;

    // Method

    public void SetData(PickupItemObject pickupItemObject)
    {
        _uniqueId = pickupItemObject.InfoData._uniqueId;
        _pos = pickupItemObject.transform.position;
    }
}

// BlockInfoData

[Serializable]
[SerializeDataPath("Data/BlockInfoData/")]
public class BlockInfoData
{
	public float _blockId = 0f;

	public List<CreationTileInfo> _creationTileInfoList = new List<CreationTileInfo>();

	public List<CreationBattleObjectInfo> _creationBattleObjectInfoList = new List<CreationBattleObjectInfo>();

	public List<CreationPickupItemObjectInfo> _creationPickupItemObjectInfoList = new List<CreationPickupItemObjectInfo>();

    // Method

	public void Clear(bool clearBlockId = true)
	{
		_blockId = 0;

		_creationTileInfoList.Clear();
		_creationBattleObjectInfoList.Clear();
		_creationPickupItemObjectInfoList.Clear();
	}

	public void AddTileObjectInfo(List<TileObject> tileObejctList)
	{
		foreach (TileObject tileObject in tileObejctList)
		{
			if (tileObject == null)
				continue;

			CreationTileInfo creationTileInfo = new CreationTileInfo();
			creationTileInfo.SetData(tileObject);

			_creationTileInfoList.Add(creationTileInfo);
		}
	}

	public void AddMonsterObjectInfo(List<MonsterObject> monsterObjectList)
	{
		foreach (MonsterObject monsterObject in monsterObjectList)
		{
			if (monsterObject == null)
				continue;
			
			CreationBattleObjectInfo creationBattleObjectInfo = new CreationBattleObjectInfo();
			creationBattleObjectInfo.SetData(monsterObject);
			
			_creationBattleObjectInfoList.Add(creationBattleObjectInfo);
		}
	}

    public void AddPickupItemObjectInfo(List<PickupItemObject> pickupItemObjectList)
    {
        foreach (PickupItemObject pickupItemObject in pickupItemObjectList)
        {
            if (pickupItemObject == null)
                continue;

            CreationPickupItemObjectInfo creationInfo = new CreationPickupItemObjectInfo();
            creationInfo.SetData(pickupItemObject);

            _creationPickupItemObjectInfoList.Add(creationInfo);
        }
    }
}