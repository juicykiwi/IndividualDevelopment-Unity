using UnityEngine;
using System.Collections;

public class StageSceneControl : SceneControl<StageSceneControl>
{
    [SerializeField]
    RunningMachine _runningMachine = null;
    public RunningMachine RunningMachine { get { return _runningMachine; } }

    [SerializeField]
    RunningMachineController _runningMachineController = null;
    public RunningMachineController RunningMachineController { get { return _runningMachineController; } }

    [SerializeField]
    Transform _runnerSpawnPos = null;

    [SerializeField]
    StageTrainerConrol _stageTrainerControl = null;

    Runner _runner = null;

    bool _startedStage = false;
    public bool StartedStage { get { return _startedStage; } }

    int _remainRetryCount = 3;

    int _stageLevel = 0;
    public int StageLevel
    {
        get { return _stageLevel; }
        set { _stageLevel = value; }
    }


    #region Override SceneControl

    protected override void Start()
    {
        base.Start();

        TouchManager.instance._touchBeganEvent += OnTouchBegan;

        UIStageControl.instance.Init();

        StageInit();
    }

    protected override void Clear()
    {
        base.Clear();

        TouchManager.instance._touchBeganEvent -= OnTouchBegan;
    }

    #endregion


    #region Start stage

    void StageInit()
    {
        // Runner
        SettingRunner(1);
        _runner.SetStartPos(_runnerSpawnPos.transform.position);

        // Running machine
        _runningMachine.Init(_runner);

        // Trainer
        _stageTrainerControl._clearedStageEvent += OnClearedStage;
        _stageTrainerControl._changedStageEvent += OnChangedStage;
        _stageTrainerControl._startedStageEvent += OnStartedStage;
        _stageTrainerControl._changedTrainingTimeRate += OnChangedTrainingTimeRate;

        _stageTrainerControl.ReadyTrainer();

        // UI Water bottle
        UIStageControl.instance.UiWaterBottleControl.VisibleWaterBottle(_remainRetryCount);

        StartStage();
    }

    public void SettingRunner(int runnerId)
    {
        runnerId = Mathf.Max(1, runnerId);

        RunnerData runnerData = RunnerDataManager.instance.GetRunnerDataById(runnerId);
        if (runnerData == null)
        {
            Debug.LogError("runnerData is null.");
            LoadLevelScene(SceneHelper.MainSceneName);
        }

        _runner = Runner.CreateRunner(runnerData);
        if (_runner == null)
        {
            Debug.LogError("_runner is null.");
            LoadLevelScene(SceneHelper.MainSceneName);
        }
    }

    void StartStage()
    {
        _startedStage = true;
        _stageTrainerControl.StartTrainerTimer(3f);
    }

    #endregion


    public void StopStage()
    {
        _startedStage = false;
        _stageTrainerControl.EndTrainer();

        UIStageControl.instance.ActiveGoMainButton(true, 2f);
    }

    public void RespawnRunner()
    {
        _runner.RunnerState = RunnerState.Spawn;

        _runner.transform.position = _runnerSpawnPos.transform.position;
        _runner.StopRunner();

        if (_remainRetryCount <= 0)
        {
            StopStage();
        }

        --_remainRetryCount;

        // UI Water bottle
        UIStageControl.instance.UiWaterBottleControl.VisibleWaterBottle(_remainRetryCount);
    }

    public bool CheckRunnerLevelUp(float runnerLevelUpValue)
    {
        if (runnerLevelUpValue <= 0f)
            return false;

        if (_stageLevel + _stageTrainerControl.CurrentTrainingTimeRate() < runnerLevelUpValue)
            return false;

        return true;
    }

    public bool CheckGetStarCoin(float starCoinRate)
    {
        if (starCoinRate <= 0f)
            return false;

        float trainingRate = _stageTrainerControl.CurrentTrainingTimeRate();
        if (trainingRate <= 0f)
            return false;

        for (float rate = trainingRate; rate >= 0f; rate -= starCoinRate)
        {
            if (rate == 0f)
            {
                return true;
            }
        }

        return false;
    }


    #region Stage event

    public void OnTouchBegan(Vector2 touchPos)
    {
        if (_startedStage == false)
        {
            return;
        }

        _runner.OnRequestRun();
    }

    #endregion


    #region StageTrainerControl Event

    public void OnClearedStage()
    {
    }

    public void OnChangedStage()
    {
    }

    public void OnStartedStage()
    {
    }

    public void OnChangedTrainingTimeRate(float rate)
    {
        /* StarCoin 획득 처리 */
        if (CheckGetStarCoin(_runner.CurrentRunnerGrade()._starCoinRateInStage) == true)
        {
            UserDataManager.instance.GetUserData()._starCoin += 1;
            UIStageControl.instance.UITopView.UpdateStarCoinCount();
        }
    }

    #endregion
}
