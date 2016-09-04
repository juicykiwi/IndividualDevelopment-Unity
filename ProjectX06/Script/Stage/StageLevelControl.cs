using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class StageLevelControl
{
    bool _active = false;
    public bool Active
    {
        get { return _active; }
        set { _active = value; }
    }

    int _level = 0;
    public int Level { get { return _level; } }

    Trainer _trainer = null;


    public void Init()
    {
        int trainerId = 1;
        TrainerData trainerData = TrainerDataManager.instance.GetTrainerDataById(trainerId);
        _trainer = Trainer.CreateTrainer(trainerData);

        if (_trainer == null)
        {
            Debug.LogError("Fail StageLevelConrol.Init(). _trainer is null.");
            return;
        }

        LevelUp();
    }

    public void Ready()
    {
    }

    public void Start()
    {
    }

    public void End()
    {
    }

    public void LevelUp()
    {
        ++_level;
//        NextTrainerData(_level);
    }

//    public TrainerData NextTrainerData(int level)
//    {
//    }
}
