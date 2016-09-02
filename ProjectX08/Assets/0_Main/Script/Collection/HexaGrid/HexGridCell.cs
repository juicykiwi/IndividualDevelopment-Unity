using UnityEngine;
using System.Collections;

public class HexGridCell
{
    HexGridPos _pos = HexGridPos.Zero;
    public HexGridPos Pos
    {
        get { return _pos; } 
        set { _pos = value; }
    }

    IHexGridData _data = null;
    public IHexGridData Data { get { return _data; } }
}
