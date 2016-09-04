using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupItemManager : Singleton<PickupItemManager>, ITitleLoadAsynk
{
    PickupItemObjectFactory _pickupItemFactory = new PickupItemObjectFactory();
    public PickupItemObjectFactory PickupItemFactory { get { return _pickupItemFactory; } }

    GameObjectPool<int, PickupItemObject> _pickupItemPool = new GameObjectPool<int, PickupItemObject>();
    public GameObjectPool<int, PickupItemObject> PickupItemPool { get { return _pickupItemPool; } }

    // Method

    #region ITitleLoadAsynk

    public void InitAsynk()
    {
        CreateInstance();
    }

    #endregion

    public void PreSettingPool(int id, int count)
    {
        for (int index = 0; index < count; ++index)
        {
            PickupItemObject pickupItemObject = _pickupItemFactory.CreatePickupItemObject(id);
            if (pickupItemObject == null)
            {
                Debug.LogError("Fail! PickupItemManager.PreSettingPool(). pickupItemObject is null.");
                break;
            }

            _pickupItemPool.Enqueue(pickupItemObject.InfoData._uniqueId, pickupItemObject);
        }
    }

    public PickupItemObject CreatePickupItemObject(int uniqueId)
    {
        PickupItemObject pickupItemObject = null;

        bool IsInGame = SceneHelper.IsInGame();
        if (IsInGame == true)
        {
            pickupItemObject = _pickupItemPool.Dequeue(uniqueId);
        }

        if (pickupItemObject == null)
        {
            pickupItemObject = _pickupItemFactory.CreatePickupItemObject(uniqueId);
        }

        return pickupItemObject;
    }

    public void Clear()
    {
        GameObjectHelper.DestroyChildAll<PickupItemObject>(transform);
        _pickupItemPool.Clear();
    }
}
