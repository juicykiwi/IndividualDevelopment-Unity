using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexaGridTestScene : MonoBehaviour
{
	void Start()
    {
        HexGridCollectionOld<int> collection1 = new HexGridCollectionOld<int>(4, 5);

        string log = "";
        for (int index = 0; index < collection1.Count; ++index)
        {
            log += collection1[index].ToString() + ", ";
        }
        Debug.Log(log);

        for (int index = 0; index < collection1.Count; ++index)
        {
            HexGridOld hexGridOld = collection1.HexaGridAtIndex(index);
            Debug.LogFormat("index {0} : columm {1} / row {2}",
                index, hexGridOld._column, hexGridOld._row);
        }

        // Collection2

        HexGridCollectionEx collection2 = new HexGridCollectionEx(5, 5);
        IDictionaryEnumerator enumerator = collection2.Enumerator;

        while (enumerator.MoveNext() == true)
        {
            HexGrid hexGrid = enumerator.Value as HexGrid;
            if (hexGrid == null)
                continue;

            Debug.LogFormat("collection2 pos x:{0} / y:{1}", hexGrid.Pos.x, hexGrid.Pos.y);
        }

        if (collection2.ContainsKey(new HexPos(0, 0)) == false)
            return;

        HexGrid hexGridTest = collection2[new HexPos(0, 0)];
        Debug.LogFormat("{0} / {1}", hexGridTest.Pos.x, hexGridTest.Pos.y);

        while (true)
        {
            if (collection2.ContainsKey(hexGridTest.Pos.RightUp) == false)
                break;
            
            hexGridTest = collection2[hexGridTest.Pos.RightUp];
            Debug.LogFormat("{0} / {1}", hexGridTest.Pos.x, hexGridTest.Pos.y);
        }

        hexGridTest = collection2[new HexPos(3, 5)];
        Debug.LogFormat("{0} / {1}", hexGridTest.Pos.x, hexGridTest.Pos.y);

        while (true)
        {
            if (collection2.ContainsKey(hexGridTest.Pos.LeftDown) == false)
                break;

            hexGridTest = collection2[hexGridTest.Pos.LeftDown];
            Debug.LogFormat("{0} / {1}", hexGridTest.Pos.x, hexGridTest.Pos.y);
        }
	}
}
