using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CustomXmlSerializerUtil;

[Serializable]
public class RunnerGrade
{
    public string _icon = "";

    public float _levelUpStage = 0;
    public float _starCoinRateInStage = 0f;

    public float _accel = 0f;
    public float _break = 0f;
    public float _stamina = 0f;
    public float _maxSpeed = 0f;
}

[Serializable]
[SerializeDataPath("Data/RunnerData/")]
public class RunnerData
{
    public string _name = "";
    public int _id = 0;

    public string _prefab = "";

    public List<RunnerGrade> _runnerGrade = new List<RunnerGrade>();
}


[Serializable]
public class RunnerDataList : DataList<RunnerData>
{
    public RunnerData FindDatabyId(int id)
    {
        return _dataList.Find(
            (RunnerData data) =>
            {
                return data._id == id;
            });
    }
}
