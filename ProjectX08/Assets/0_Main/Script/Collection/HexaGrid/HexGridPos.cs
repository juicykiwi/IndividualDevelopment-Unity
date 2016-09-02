using UnityEngine;
using System.Collections;

/*
 * 
( 0, 8)         ( 2, 8)         ( 4, 8)
        ( 1, 7)         ( 3, 7)
( 0, 6)         ( 2, 6)         ( 4, 6)
        ( 1, 5)         ( 3, 5)
( 0, 4)         ( 2, 4)         ( 4, 4)
        ( 1, 3)         ( 3, 3)
( 0, 2)         ( 2, 2)         ( 4, 2)
        ( 1, 1)         ( 3, 1)
( 0, 0)         ( 2, 0)         ( 4, 0)


*/

public struct HexGridPos
{
    public int x;
    public int y;

    public HexGridPos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static HexGridPos Zero { get { return new HexGridPos(); } }

    public HexGridPos Up        { get { return new HexGridPos(x,        y + 2   ); } }
    public HexGridPos Down      { get { return new HexGridPos(x,        y - 2   ); } }
    public HexGridPos RightUp   { get { return new HexGridPos(x + 1,    y + 1   ); } }
    public HexGridPos RightDown { get { return new HexGridPos(x + 1,    y - 1   ); } }
    public HexGridPos LeftUp    { get { return new HexGridPos(x - 1,    y + 1   ); } }
    public HexGridPos LeftDown  { get { return new HexGridPos(x - 1,    y - 1   ); } }
}
