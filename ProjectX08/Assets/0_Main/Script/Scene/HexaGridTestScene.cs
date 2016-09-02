using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexaGridTestScene : MonoBehaviour
{
	void Start()
    {
        // Collection2

        HexGridCollection collection2 = new HexGridCollection(8, 8);
        IDictionaryEnumerator enumerator = collection2.Enumerator;

        while (enumerator.MoveNext() == true)
        {
            HexGridCell hexGridCell = enumerator.Value as HexGridCell;
            if (hexGridCell == null)
                continue;

            Debug.LogFormat("collection2 pos x:{0} / y:{1}", hexGridCell.Pos.x, hexGridCell.Pos.y);
        }

        if (collection2.ContainsKey(new HexGridPos(0, 0)) == false)
            return;

        HexGridCell hexGridTest = collection2[new HexGridPos(0, 0)];
        Debug.LogFormat("{0} / {1}", hexGridTest.Pos.x, hexGridTest.Pos.y);

        while (true)
        {
            if (collection2.ContainsKey(hexGridTest.Pos.RightUp) == false)
                break;
            
            hexGridTest = collection2[hexGridTest.Pos.RightUp];
            Debug.LogFormat("{0} / {1}", hexGridTest.Pos.x, hexGridTest.Pos.y);
        }

        hexGridTest = collection2[new HexGridPos(3, 5)];
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
