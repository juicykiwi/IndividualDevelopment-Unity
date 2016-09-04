using UnityEngine;
using System;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class UserDataManager : DataManager<UserDataManager>
{
    [SerializeField]
    UserDataList _userDataList = new UserDataList();

    public override void ClearData()
    {
        _userDataList.Clear();
    }

    public override void LoadData()
    {
        ClearData();
        _userDataList.LoadData("", true);

        if (_userDataList.Count <= 0)
        {
            _userDataList.GetList().Add(new UserData());
            SaveData();
        }
    }

    public override void SaveData()
    {
        _userDataList.SaveData("", true);
        RefreshAssetData();
    }

    public UserData GetUserData()
    {
        return _userDataList[0];
    }

    public void SetLastSelectedRunnerID(int id)
    {
        _userDataList[0]._lastSelectedRunnerId = id;
    }

    public int GetLastSelectedRunnerID()
    {
        if (_userDataList[0]._lastSelectedRunnerId <= 0)
            return 1;
        
        return _userDataList[0]._lastSelectedRunnerId;
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(UserDataManager))]
public class UserDataManagerEditor : DataManagerEditor<UserDataManager>
{
}

#endif
