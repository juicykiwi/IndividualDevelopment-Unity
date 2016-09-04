using UnityEngine;
using System.Collections;

public class StageDataManager : DataManager<StageDataManager, StageInfoData>, IDataManager
{
    public void LoadData()
    {
        LoadData("StageInfoData", DataStorageType.Resources);
    }

    public void SaveData()
    {
        SaveData("StageInfoData", DataStorageType.Resources);
    }
}
