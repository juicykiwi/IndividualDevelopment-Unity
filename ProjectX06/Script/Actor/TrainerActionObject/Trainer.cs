using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Trainer : BaseActor
{
    [SerializeField]
    TrainerAnimatorControl _animatorControl = null;

    TrainerData _trainerData = null;
    public TrainerData TrainerData
    {
        get { return _trainerData; }
        set { _trainerData = value; } 
    }

    bool _active = false;
    public bool IsActive { get { return _active; } }

    float _trainingMenuTime = 0f;

    float _trainingTotalTime = 0f;
    public float TrainingTotalTime { get { return _trainingTotalTime; } }

    RunningMachine _runningMachine = null;
    RunningMachineController _runningMachineConroller = null;

    TrainingMenu _currentTrainingMenu = null;
    Queue<TrainingMenu> _trainingMenuQueue = new Queue<TrainingMenu>();

    public Action UpdatedTrainingTimeEvent = null;
    public Action FinishedTrainingEvent = null;

    
    public static Trainer CreateTrainer(TrainerData trainerData)
    {
        if (trainerData == null)
            return null;

        GameObject trainerPrefab = TrainerDataManager.instance._prefabDataDict.GetPrefabByName(trainerData._prefab);
        if (trainerPrefab == null)
            return null;

        GameObject trainerGameObject = Instantiate(trainerPrefab) as GameObject;
        Trainer trainer = trainerGameObject.GetComponent<Trainer>();
        if (trainer == null)
            return null;

        trainer.TrainerData = trainerData;

        return trainer;
    }


    #region MonobeHaviour message

    void FixedUpdate()
    {
        if (_active == false)
            return;

        _trainingMenuTime += Time.fixedDeltaTime;
        _trainingTotalTime += Time.fixedDeltaTime;

        if (UpdatedTrainingTimeEvent != null)
        {
            UpdatedTrainingTimeEvent();
        }

        if (_trainingTotalTime >= _trainerData._trainingTime)
        {
            if (FinishedTrainingEvent != null)
            {
                FinishedTrainingEvent();
            }

            return;
        }

        ChangeTraningMenuProcess();
    }

    #endregion

    public void Active(bool active)
    {
        if (_active == active)
            return;
        
        _active = active;

        if (_active == true)
        {
            _runningMachine.StartRunningMachine();

            if (_currentTrainingMenu._velocity > _runningMachine.Speed)
            {
                RunningMachineOrder_Fast();
            }
            else
            {
                RunningMachineOrder_Slow();
            }
        }
        else
        {
            _runningMachine.StopRunningMachine();
        }
    }


    #region FixedUpdate process

    void ChangeTraningMenuProcess()
    {
        /* 트레이닝 메뉴 변경 */

        if (_trainingMenuTime < _currentTrainingMenu._menuTime)
            return;

        if (_trainingMenuQueue.Count <= 0)
        {
            SettingTrainingMenuQueue();
        }

        TrainingMenu oldMenu = _currentTrainingMenu;
        _currentTrainingMenu = _trainingMenuQueue.Dequeue();
        SetRunningMachineMenu(_currentTrainingMenu);

        _trainingMenuTime = 0f;

        bool isAccelButton = (oldMenu._velocity < _currentTrainingMenu._velocity);
        if (isAccelButton == true)
        {
            RunningMachineOrder_Fast();
        }
        else
        {
            RunningMachineOrder_Slow();
        }
    }

    #endregion


    public void Init(RunningMachine runningMachine, RunningMachineController _conroller)
    {
        if (runningMachine == null)
            return;

        _runningMachine = runningMachine;
        _runningMachineConroller = _conroller;

        SettingTrainingMenuQueue();

        _currentTrainingMenu = _trainingMenuQueue.Dequeue();
        SetRunningMachineMenu(_currentTrainingMenu);
    }

    public void SettingTrainingMenuQueue()
    {
        if (_trainerData == null)
            return;

        for (int index = 0; index < _trainerData._trainingMenuList.Count; ++index)
        {
            if (_trainerData._trainingMenuList[index] == null)
                continue;

            _trainingMenuQueue.Enqueue(_trainerData._trainingMenuList[index]);
        }
    }

    void SetRunningMachineMenu(TrainingMenu trainingMenu)
    {
        _runningMachine.SetTrainingMenu(trainingMenu);
    }

    void RunningMachineOrder_Fast()
    {
        _animatorControl.SelectRAnimation(1, 0.5f);
        _runningMachineConroller.FastButtonDown(1, 0.5f);
    }

    void RunningMachineOrder_Slow()
    {
        _animatorControl.SelectLAnimation(1, 0.5f);
        _runningMachineConroller.SlowButtonDown(1, 0.5f);
    }

    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }
}
