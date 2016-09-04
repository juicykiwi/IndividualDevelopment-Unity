using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockManager : Singleton<BlockManager>, ITitleLoadAsynk
{
	public List<BlockObject> _blockObjectList = new List<BlockObject>();

	// Method

	public void Init()
	{
		if (BlockDataManager.instance.IsLoadData == false)
		{
			BlockDataManager.instance.Init();
		}
	}

    #region ITitleLoadAsynk

    public void InitAsynk()
    {
        CreateInstance();
    }

    #endregion

	public void AddBlockObject(BlockObject blockObject)
	{
		blockObject.transform.SetParent(transform);

		_blockObjectList.Add(blockObject);
	}

	public void RemoveBlockObject(BlockObject blockObject)
	{
		_blockObjectList.Remove(blockObject);
	}

	public void RemoveBlockObjectAll()
	{
		_blockObjectList.Clear();
		GameObjectHelper.DestroyChildAll<BlockObject>(transform);
	}
}
