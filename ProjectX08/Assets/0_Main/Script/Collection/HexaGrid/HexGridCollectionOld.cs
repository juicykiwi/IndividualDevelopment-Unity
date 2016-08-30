using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexGridCollectionOld<T>
{
    const int TwoLine = 2;

    public int BaseWidth { get; private set; }
    public int BaseHeight { get; private set; }

    List<T> _list = null;
    public int Count { get { return _list.Count; } }

    public T this[int index]
    {
        get { return _list[index]; }
        set { _list[index] = value; }
    }

    public HexGridCollectionOld(int width, int height)
    {
        BaseWidth = width;
        BaseHeight = height;

        _list = new List<T>(FullCapacityAtLine(BaseWidth, height));

        for (int index = 0; index < _list.Capacity; ++index)
        {
            _list.Add(default(T));
        }
    }

    int FullCapacityAtLine(int line)
    {
        return FullCapacityAtLine(BaseWidth, line);
    }

    int FullCapacityAtLine(int baseWidth, int line)
    {
        return (baseWidth * line) - (line / TwoLine);
    }

    public int IndexAtHexaGrid(HexGridOld grid)
    {
        if (IsValidHexaGrid(grid) == false)
            return -1;

        return FullCapacityAtLine(grid._row) + grid._column;
    }

    int TwoLineCount()
    {
        return (BaseWidth * TwoLine - 1);
    }

    public int Column(int index)
    {
        return index % TwoLineCount() % BaseWidth;
    }

    public int Row(int index)
    {
        int TwoLineToOneLine = index / TwoLineCount() * 2;
        int addRow = index % TwoLineCount() / BaseWidth;

        return TwoLineToOneLine + addRow;
    }

    public HexGridOld HexaGridAtIndex(int index)
    {
        if (IsValidIndex(index) == false)
            return HexGridOld.InvalidHexaGrid;
        
        HexGridOld grid = new HexGridOld();
        grid._column = Column(index);
        grid._row = Row(index);

        return grid;
    }

    public bool IsValidHexaGrid(HexGridOld grid)
    {
        if (grid._column < 0 || grid._row < 0)
            return false;

        if (grid._column >= BaseWidth || grid._row >= BaseHeight)
            return false;

        if (grid._row % TwoLine == 1)
        {
            if (grid._column >= BaseWidth - 1)
                return false;
        }

        return true;
    }

    public bool IsValidIndex(int index)
    {
        if (index < 0)
            return false;

        if (index >= Count)
            return false;

        return true;
    }
}
