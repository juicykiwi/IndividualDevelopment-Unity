using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class BlockToolTileBrush : IBlockToolBrush
{
    public int _tileId = 0;
    public int ElementId
    {
        get { return _tileId; }
        set { _tileId = value; }
    }

    public IBlockElement Draw(Vector2 pos)
    {
        TileObject tileObject = TileManager.instance.CreateTile(ElementId);
        if (tileObject == null)
            return null;

        tileObject.transform.position = pos;
        return tileObject;
    }
}
