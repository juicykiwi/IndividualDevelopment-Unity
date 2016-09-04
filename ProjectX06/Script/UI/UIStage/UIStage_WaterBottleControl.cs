using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIStage_WaterBottleControl : MonoBehaviour
{
    [SerializeField]
    List<Image> _waterBottleList = new List<Image>();


    public void VisibleWaterBottle(int visibleCount)
    {
        for (int index = _waterBottleList.Count - 1; index >= 0; --index)
        {
            if (_waterBottleList[index] == null)
                continue;

            _waterBottleList[index].enabled = (index < visibleCount);
        }
    }
}
