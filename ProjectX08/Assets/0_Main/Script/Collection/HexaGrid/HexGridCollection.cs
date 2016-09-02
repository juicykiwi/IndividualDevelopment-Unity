using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexGridCollection
{
    public Dictionary<HexGridPos, HexGridCell> _hexGridDict = null;

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

    public HexGridCollection(int width = 5, int height = 5)
    {
        Width = width;
        Height = height;

        _hexGridDict = new Dictionary<HexGridPos, HexGridCell>(GridMaxCount);

        for (int x = 0; x <= width; ++x)
        {
            int startY = StartYAtX(x);
            int endY = EndYAtX(x);

            for (int y = startY; y <= endY; y += 2)
            {
                HexGridCell hexGridCell = new HexGridCell();
                hexGridCell.Pos = new HexGridPos(x, y);

                _hexGridDict.Add(hexGridCell.Pos, hexGridCell);
            }
        }
    }

    public int CountHeightAtX(int x)
    {
        if (Height < 0)
            return 0;

        return ((Height - StartYAtX(x)) / 2) + 1;
    }

    public int StartYAtX(int x)
    {
        return x % 2;
    }

    public int EndYAtX(int x)
    {
        return Height - StartYAtX(x);
    }

    public bool ContainsKey(HexGridPos hexGridPos)
    {
        return _hexGridDict.ContainsKey(hexGridPos);
    }

    public HexGridCell this[HexGridPos hexGridPos]
    {
        get
        {
            return _hexGridDict[hexGridPos];
        }
    }
}
