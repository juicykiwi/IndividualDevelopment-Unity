using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class StageTrainerConrol : MonoBehaviour
{
    [SerializeField]
    Transform _trainerSpawnPos = null;

    Trainer _trainer = null;
    public Trainer Trainer { get { return _trainer; } }

    TrainerData _trainerData = null;

    /* Event */
    public Action _clearedStageEvent = null;
    public Action _changedStageEvent = null;
    public Action _startedStageEvent = null;

    public Action<float> _changedTrainingTimeRate = null;


    public void ReadyTrainer()
    {
        int stageLevel = StageSceneControl.instance.StageLevel;
        
        _trainerData = NextLevelTrainerData(stageLevel + 1);
        if (_trainerData != null)
        {
            StageSceneControl.instance.StageLevel = stageLevel + 1;
        }
        else
        {
            _trainerData = NextLevelTrainerData(stageLevel);
        }

        ChangeTrainer(_trainerData);

        UIStageControl.instance.UIStageLevelSlider.value = 0;
        UIStageControl.instance.UIStageLevelSlider.maxValue = 1f;

        if (_changedStageEvent != null)
        {
            _changedStageEvent();
        }
    }

    public void StartTrainerTimer(float time)
    {
        UIStageControl.instance.UIStage_Countdown.CountdownAction(time, true, OnCompleteCountdown);
    }

    public void OnCompleteCountdown()
    {
        StartTrainer();

        if (_startedStageEvent != null)
        {
            _startedStageEvent();
        }
    }
   
    public void StartTrainer()
    {
        _trainer.Active(true);
    }

    public void EndTrainer()
    {
        _trainer.Active(false);
    }

    public TrainerData NextLevelTrainerData(int level)
    {
        return TrainerDataManager.instance.GetRandomTrainerDataByLevel(level);
    }

    public void ChangeTrainer(TrainerData trainerData)
    {
        if (trainerData == null)
            return;

        if (_trainer != null)
        {
            _trainer.FinishedTrainingEvent = null;

            Destroy(_trainer.gameObject);
            _trainer = null;
        }

        _trainer = Trainer.CreateTrainer(trainerData);
        if (_trainer == null)
        {
            Debug.LogError("Fail StageTrainerConrol.ChangeTrainer(). _trainer is null.");
        }

        _trainer.SetPos(_trainerSpawnPos.transform.position);

        _trainer.Init(
            StageSceneControl.instance.RunningMachine,
            StageSceneControl.instance.RunningMachineController);

        _trainer.UpdatedTrainingTimeEvent += OnUpdatedTrainingTime;
        _trainer.FinishedTrainingEvent += OnFinishedTraining;
    }

    public float CurrentTrainingTimeRate()
    {
        if (_trainer == null)
            return 0f;

        // 5%씩 게이지 채우는 처리
        const float increaseOffset = 20f;
        const float decreaseOffset = 0.05f;

        float currentRate = _trainer.TrainingTotalTime / _trainer.TrainerData._trainingTime;
        currentRate = (int)(currentRate * increaseOffset) * decreaseOffset;
        currentRate = Mathf.Min(currentRate, 1f);

        return currentRate;
    }


    #region Trainer Event

    public void OnUpdatedTrainingTime()
    {
        if (_trainer != null)
        {
            if (_trainer.IsActive == true)
            {
                float newTrainingTimeRate = CurrentTrainingTimeRate();
                if (newTrainingTimeRate != UIStageControl.instance.UIStageLevelSlider.value)
                {
                    UIStageControl.instance.UIStageLevelSlider.value = newTrainingTimeRate;
                    if (_changedTrainingTimeRate != null)
                    {
                        _changedTrainingTimeRate(newTrainingTimeRate);
                    }
                }
            }
        }
    }

    public void OnFinishedTraining()
    {
        _trainer.Active(false);

//        Invoke("ReadyTrainer", 1f);
//        StartTrainerTimer(3f);

        ReadyTrainer();
        OnCompleteCountdown();

        if (_clearedStageEvent != null)
        {
            _clearedStageEvent();
        }
    }

    #endregion
}
