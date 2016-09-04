using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class UserData
{
    public string _name = "";
    public int _id = 0;

    public int _lastSelectedRunnerId = 0;

    public int _starCoin = 0;
}

[Serializable]
public class UserDataList : DataList<UserData>
{
}
