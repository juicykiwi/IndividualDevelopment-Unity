using UnityEngine;
using System.Collections;

public class HexGrid
{
    HexPos _pos = HexPos.Zero;
    public HexPos Pos
    {
        get { return _pos; } 
        set { _pos = value; }
    }

    HexGridData _data = null;
    public HexGridData Data { get { return _data; } }
}
