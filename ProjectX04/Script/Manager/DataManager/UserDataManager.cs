using UnityEngine;
using System.Collections;

public class UserDataManager : DataManager<UserDataManager, UserInfoData>, IDataManager
{
    public void LoadData()
    {
        LoadData("UserInfoData", DataStorageType.Persistent);
    }

    public void SaveData()
    {
        SaveData("UserInfoData", DataStorageType.Persistent);
    }

    public void SetUserInfo(UserInfoData data)
    {
        ClearData();

        DataList.Add(data);
    }

    public UserInfoData DataById(int id)
    {
        return DataList.Find(
            (UserInfoData data) => { return data._id == id; });
    }
}
