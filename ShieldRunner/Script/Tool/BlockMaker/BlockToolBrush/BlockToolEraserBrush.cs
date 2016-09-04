using UnityEngine;
using System.Collections;

public class BlockToolEraserBrush : IBlockToolBrush
{
    int _eraserId = 0;
    public int ElementId
    {
        get { return _eraserId; }
        set { _eraserId = value; }
    }

    public IBlockElement Draw(Vector2 pos)
    {
        return null;
    }
}
