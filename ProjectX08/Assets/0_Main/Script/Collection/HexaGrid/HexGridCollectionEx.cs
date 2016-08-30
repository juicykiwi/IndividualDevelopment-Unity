using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexGridCollectionEx
{
    public Dictionary<HexPos, HexGrid> _hexGridDict = null;

    public int Width { get; private set; }
    public int Height { get; private set; }

    public int Count { get { return _hexGridDict.Count; } }
    public int GridMaxCount { get { return Width * Height - (Width / 2); } }

    public IDictionaryEnumerator Enumerator
    {
        get
        { 
            return _hexGridDict.GetEnumerator();
        } 
    }

    public HexGridCollectionEx(int width = 5, int height = 5)
    {
        Width = width;
        Height = height;

        _hexGridDict = new Dictionary<HexPos, HexGrid>(GridMaxCount);

        for (int x = 0; x < width; ++x)
        {
            int startY = StartYAtX(x);
            int maxY = CountHeightAtX(x) + startY;

            for (int y = startY; y < maxY; ++y)
            {
                HexGrid hexGrid = new HexGrid();
                hexGrid.Pos = new HexPos(x, y);

                _hexGridDict.Add(hexGrid.Pos, hexGrid);
            }
        }
    }

    public int CountHeightAtX(int x)
    {
        return Height - (x % 2);
    }

    public int StartYAtX(int x)
    {
        return x - (x / 2);
    }

    public bool ContainsKey(HexPos hexPos)
    {
        return _hexGridDict.ContainsKey(hexPos);
    }

    public HexGrid this[HexPos hexPos]
    {
        get
        {
            return _hexGridDict[hexPos];
        }
    }
}
