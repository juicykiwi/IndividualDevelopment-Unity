using UnityEngine;
using System.Collections;

/*

( 0, 4)         ( 2, 5)         ( 4, 6)
        ( 1, 4)         ( 3, 5)
( 0, 3)         ( 2, 4)         ( 4, 5)
        ( 1, 3)         ( 3, 4)
( 0, 2)         ( 2, 3)         ( 4, 4)
        ( 1, 2)         ( 3, 3)
( 0, 1)         ( 2, 2)         ( 4, 3)
        ( 1, 1)         ( 3, 2)
( 0, 0)         ( 2, 1)         ( 4, 2)

*/

public struct HexPos
{
    public int x;
    public int y;

    public HexPos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static HexPos Zero { get { return new HexPos(); } }

    public HexPos Up { get { return new HexPos(x, y + 1); } }
    public HexPos Down { get { return new HexPos(x, y - 1); } }
    public HexPos RightUp { get { return new HexPos(x + 1, y + 1); } }
    public HexPos RightDown { get { return new HexPos(x + 1, y); } }
    public HexPos LeftUp { get { return new HexPos(x - 1, y); } }
    public HexPos LeftDown { get { return new HexPos(x - 1, y - 1); } }
}
