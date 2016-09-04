using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class BlockTileControl : BlockElementControl<CreationTileInfo, TileObject>
{    
    protected override TileObject CreateElement(CreationTileInfo creationInfo)
    {
        if (creationInfo == null)
            return null;

        TileObject tileObject = TileManager.instance.CreateTile(creationInfo._uniqueId);
        if (tileObject == null)
            return null;

        tileObject.transform.SetParent(_parentBlockObject.transform);
        tileObject.SetLocalPos(GameObjectHelper.RoundPosition2D(creationInfo._pos));

        _parentBlockObject.UpdateBountRect(tileObject.transform);

        return tileObject;
    }

    public override void Add(TileObject tObject)
    {
        if (tObject == null)
            return;

        tObject.transform.SetParent(_parentBlockObject.transform);
        tObject.SetLocalPos(GameObjectHelper.RoundPosition2D(tObject.transform.localPosition));

        _parentBlockObject.UpdateBountRect(tObject.transform);

        _objectList.Add(tObject);
    }

    public override void Remove(TileObject tObject)
    {
        if (tObject == null)
            return;

        _objectList.Remove(tObject);
        tObject.Remove();
    }

    public void Remove(int createdTileId)
    {
        TileObject findTileObject = _objectList.Find(
            (TileObject tileObject) => { return tileObject.CreatedTileId == createdTileId; }
        );

        Remove(findTileObject);
    }
}
