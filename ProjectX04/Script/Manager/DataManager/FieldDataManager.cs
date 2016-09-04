using UnityEngine;
using System.Collections;

public class FieldDataManager : DataManager<FieldDataManager, FieldInfoData>, IDataManager
{
    const int FieldCountMax = 5;

    public void LoadData()
    {
        for (int index = 0; index < FieldCountMax; ++index)
        {
            string postfix = string.Format("{0}", (index + 1).ToString("D3"));
            LoadData("FieldInfoData" + postfix, DataStorageType.Resources);
        }
    }

    public void SaveData()
    {
    }

    public void SaveData(int fieldIndex)
    {
        string postfix = string.Format("{0}", (fieldIndex + 1).ToString("D3"));
        SaveData("FieldInfoData" + postfix, DataStorageType.Resources);
    }

    public FieldInfoData DataById(int id)
    {
        return DataList.Find(
            (FieldInfoData data) => { return data._id == id; } );
    }
}
