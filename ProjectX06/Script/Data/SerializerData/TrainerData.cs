using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CustomXmlSerializerUtil;

[Serializable]
public class TrainingMenu
{
    public float _menuTime = 0f;

    // 트레이너가 조작하는 가속도 
    public float _accelForce = 0f;

    // 트레이너가 원하는 속도
    public float _velocity = 0f;


    public float Velocity()
    {
        return _velocity;
    }

    public float AccelForce()
    {
        return _accelForce;
    }
}

[Serializable]
[SerializeDataPath("Data/TrainerData/")]
public class TrainerData
{
    public string _name = "";
    public int _id = 0;

    public string _prefab = "";
    public string _icon = "";

    // 어려움의 강도를 나타내는 값
    public int _level = 0;

    // 훈련 총 총 시간
    public float _trainingTime = 0f;

    // 훈련 메뉴
    public List<TrainingMenu> _trainingMenuList = new List<TrainingMenu>();
}

[Serializable]
public class TrainerDataList : DataList<TrainerData>
{
    public TrainerData FindDatabyId(int id)
    {
        return _dataList.Find(
            (TrainerData data) =>
            {
                return data._id == id;
            });
    }
}
