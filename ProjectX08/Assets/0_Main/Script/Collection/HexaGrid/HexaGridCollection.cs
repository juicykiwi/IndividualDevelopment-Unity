using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexaGridCollection<T>
{
    public int BaseWidth { get; private set; }
    public int BaseHeight { get; private set; }

    List<T> _list = null;
    public int Count { get { return _list.Count; } }

    public T this[int index]
    {
        get { return _list[index]; }
        set { _list[index] = value; }
    }

    public HexaGridCollection(int width, int height)
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
        return (baseWidth * line) - (line / 2);
    }

    public int IndexAtHexaGrid(HexaGrid grid)
    {
        if (IsValidHexaGrid(grid) == false)
            return -1;

        return FullCapacityAtLine(grid._row) + grid._column;
    }

    public int Column(int index)
    {
        return index % (BaseWidth * 2 - 1) % BaseWidth;
    }

    public int Row(int index)
    {
        int expression = (BaseWidth * 2 - 1);

        int baseRow = index / expression * 2;
        int addRow = index % expression / BaseWidth;

        return baseRow + addRow;
    }

    public HexaGrid HexaGridAtIndex(int index)
    {
        if (IsValidIndex(index) == false)
            return HexaGrid.InvalidHexaGrid;
        
        HexaGrid grid = new HexaGrid();
        grid._column = Column(index);
        grid._row = Row(index);

        return grid;
    }

    public bool IsValidHexaGrid(HexaGrid grid)
    {
        if (grid._column < 0 || grid._column >= BaseWidth)
            return false;
        
        if (grid._row < 0 || grid._row >= BaseHeight)
            return false;

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
