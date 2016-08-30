using UnityEngine;
using System.Collections;

/*  0   1   2   3
 *    4   5   6
 *  7   8   9  10
 *   11  12  13
 * 14  15  16  17
 *   18  19  20
 */

/// <summary>
/// Hexa grid. column and row start index is zero.
/// </summary>
public struct HexGridOld
{
    public int _column;
    public int _row;

    public static HexGridOld InvalidHexaGrid
    {
        get { return new HexGridOld(-1, -1); }
    }

    public HexGridOld(HexGridOld grid)
    {
        _column = grid._column;
        _row = grid._row;
    }

    public HexGridOld(int column, int row)
    {
        _column = column;
        _row = row;
    }

    public void Up()
    {
        if (_row < 2)
            return;

        _row -= 2;
    }

    public void Down()
    {
        _row += 2;
    }

    public void LeftUp()
    {
        if (_row % 2 == 0)
            _column -= 1;

        _row -= 1;
    }

    public void LeftDown()
    {
        if (_row % 2 == 0)
            _column -= 1;

        _row += 1;
    }

    public void RightUp()
    {
        if (_row % 2 == 1)
            _column += 1;

        _row -= 1;
    }

    public void RightDown()
    {
        if (_row % 2 == 1)
            _column += 1;

        _row += 1;
    }

    public bool IsValid()
    {
        return (_column >= 0 && _row >= 0);
    }
}
