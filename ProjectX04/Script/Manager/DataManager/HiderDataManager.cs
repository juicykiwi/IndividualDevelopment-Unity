using UnityEngine;
using System.Collections;

public class HiderDataManager : DataManager<HiderDataManager, HiderInfoData>, IDataManager
{
    public void LoadData()
    {
        LoadData("HiderInfoData", DataStorageType.Resources);
    }

    public void SaveData()
    {
        SaveData("HiderInfoData", DataStorageType.Resources);
    }
}
