using UnityEngine;
using System.Collections;

public class ChaModelManager : DataManager<ChaModelManager, ChaModelData>, IDataManager
{
    public void LoadData()
    {
        LoadData("ChaModelData", DataStorageType.Resources);
    }

    public void SaveData()
    {
        SaveData("ChaModelData", DataStorageType.Resources);
    }

    public ChaModelData DataById(int id)
    {
        return DataList.Find(
            (ChaModelData data) => { return data._id == id; } );
    }
}
