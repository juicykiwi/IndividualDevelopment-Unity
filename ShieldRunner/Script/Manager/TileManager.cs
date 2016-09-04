using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : Singleton<TileManager>, ITitleLoadAsynk
{
    // Factory
    TileObjectFactory _factory = new TileObjectFactory();
    public TileObjectFactory Factory { get { return _factory; } }

    GameObjectPool<int, TileObject> _tilePool = new GameObjectPool<int, TileObject>();
    public GameObjectPool<int, TileObject> TilePool { get { return _tilePool; } }

    // Method

    #region ITitleLoadAsynk

    public void InitAsynk()
    {
        CreateInstance();
    }

    #endregion

    public void PreSettingPool(int tileId, int count)
    {
        for (int index = 0; index < count; ++index)
        {
            TileObject tileObject = _factory.CreateTileObject(tileId);
            if (tileObject == null)
            {
                Debug.LogError("Fail! TileManager.PreSettingPool(). tileObject is null.");
                break;
            }
            
            _tilePool.Enqueue(tileObject.InfoData._uniqueId, tileObject);
        }
    }

    public TileObject CreateTile(int uniqueId)
    {
        TileObject tileObject = null;

        if (SceneHelper.IsInGame() == true)
        {
            tileObject = _tilePool.Dequeue(uniqueId);
        }

        if (tileObject == null)
        {
            tileObject = _factory.CreateTileObject(uniqueId);
        }

        return tileObject;
    }

    public void Clear()
    {
        GameObjectHelper.DestroyChildAll<TileObject>(transform);
        _tilePool.Clear();
    }
}
