using UnityEngine;
using System.Collections;

public class ChaDataManager : DataManager<ChaDataManager, ChaInfoData>, IDataManager
{    
    public void LoadData()
    {
        LoadData("ChaInfoData", DataStorageType.Resources);
    }

    public void SaveData()
    {
        SaveData("ChaInfoData", DataStorageType.Resources);
    }

    public ChaInfoData DataById(int id)
    {
        return DataList.Find(
            (ChaInfoData data) => { return data._id == id; } );
    }
}
