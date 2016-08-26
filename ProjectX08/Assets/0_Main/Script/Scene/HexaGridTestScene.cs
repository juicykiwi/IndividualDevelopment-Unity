using UnityEngine;
using System.Collections;

public class HexaGridTestScene : MonoBehaviour
{
	void Start()
    {
        HexaGridCollection<int> collection1 = new HexaGridCollection<int>(4, 5);

        string log = "";
        for (int index = 0; index < collection1.Count; ++index)
        {
            log += collection1[index].ToString() + ", ";
        }
        Debug.Log(log);

        for (int index = 0; index < collection1.Count; ++index)
        {
            HexaGrid hexaGrid = collection1.HexaGridAtIndex(index);
            Debug.LogFormat("index {0} : columm {1} / row {2}",
                index, hexaGrid._column, hexaGrid._row);
        }
	}
}
