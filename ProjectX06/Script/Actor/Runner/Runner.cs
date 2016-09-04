using UnityEngine;
using System.Collections;

public class Runner : BaseActor
{
    [SerializeField]
    protected Rigidbody2D _rigidbody = null;

    /* Runner Data */

    [SerializeField]
    RunnerData _runnerData = null;
    public RunnerData RunnerData { get { return _runnerData; } }

    int _runnerGrade = 1;

    [SerializeField]
    RunnerModelControl _modelControl = null;

    [SerializeField]
    RunnerAnimatorControl _animatorControl = null;

    [SerializeField]
    bool _landed = false;
    public bool Landed { get { return _landed; } }

    [SerializeField]
    RunnerState _runnerState = RunnerState.Spawn;
    public RunnerState RunnerState
    { 
        get { return _runnerState; }
        set { _runnerState = value; } 
    }

    [SerializeField]
    float _runnerSpeed = 0f;

    [SerializeField]
    float _runningMachineSpeed = 0f;

    [SerializeField]
    float _requestRunDeltaTime = 0f;

    [SerializeField]
    float _requestSpeed = 0f;


    public static Runner CreateRunner(RunnerData runnerData)
    {
        if (runnerData == null)
            return null;

        GameObject runnerPrefab = RunnerDataManager.instance._prefabDataDict.GetPrefabByName(runnerData._prefab);
        if (runnerPrefab == null)
            return null;

        GameObject runnerGameObject = Instantiate(runnerPrefab) as GameObject;
        Runner runner = runnerGameObject.GetComponent<Runner>();
        if (runner == null)
            return null;

        runner.Init(runnerData);

        return runner;
    }

    void Update()
    {
        /* Runner Update */

        switch (_runnerState)
        {
            case RunnerState.Running:
                break;

            default:
                return;
        }

        _requestRunDeltaTime += Time.deltaTime;

        // 가속
        if (CurrentRunnerGrade()._stamina >= _requestRunDeltaTime)
        {
            float requestSpeedDiff = _requestSpeed - _runnerSpeed;
            _runnerSpeed += requestSpeedDiff * Time.deltaTime * CurrentRunnerGrade()._stamina;
            _runnerSpeed = Mathf.Min(_runnerSpeed, CurrentRunnerGrade()._maxSpeed);
        }
        else
        {
            _requestSpeed = _runnerSpeed;
        }

        // 감속
        if (_runnerSpeed > 0f)
        {
            if (CurrentRunnerGrade()._stamina < _requestRunDeltaTime)
            {
                _runnerSpeed -= CurrentRunnerGrade()._break * Time.deltaTime;
                _runnerSpeed = Mathf.Max(_runnerSpeed, 0f);
            }
        }

        if (_runnerSpeed > 0f && _landed == true)
        {
            _modelControl.SetAnimation(RunnerAnimateType.Run);
        }
        else
        {
            _modelControl.SetAnimation(RunnerAnimateType.Idle);
        }

        float speedDiff = _runnerSpeed - _runningMachineSpeed;
        transform.Translate(speedDiff * Time.deltaTime, 0f, 0f);
    }

    public void Init(RunnerData runnerData)
    {
        _runnerData = runnerData;

        _runnerGrade = 1;
        _modelControl.ActiveModel(_runnerGrade);
    }

    public void SetStartPos(Vector3 startPos)
    {
        transform.position = startPos;
    }

    public void SetLanded(bool landed)
    {
        _landed = landed;

        if (_landed == true &&
            _runnerState == RunnerState.Spawn)
        {
            _runnerState = RunnerState.Running;
            _runnerSpeed = _runningMachineSpeed;
        }
    }

    public void StopRunner()
    {
        _runnerSpeed = 0f;
    }

    public void SetRunnerMachineSpeed(float speed, bool force)
    {
        if (force = false)
        {
            if (_landed == false)
                return;
        }

        _runningMachineSpeed = speed;
    }

    public void OnRequestRun()
    {
        if (_landed == false)
            return;
        
        _requestRunDeltaTime = 0f;
        _requestSpeed += CurrentRunnerGrade()._accel;
        _requestSpeed = Mathf.Min(_requestSpeed, CurrentRunnerGrade()._maxSpeed * 2f);
    }
        
    public void OnRunningMachineStandBoune(Vector2 bounceEffectSpeed)
    {
        _runnerSpeed = 0f;
        _requestSpeed = 0f;
        _runningMachineSpeed = bounceEffectSpeed.x;

        _rigidbody.velocity = new Vector2(0f, bounceEffectSpeed.y);
    }

    public RunnerGrade CurrentRunnerGrade()
    {
        if (_runnerGrade <= 0)
            return null;

        if (_runnerData._runnerGrade.Count < _runnerGrade)
            return null;
        
        return _runnerData._runnerGrade[_runnerGrade - 1];
    }

    public RunnerGrade NextRunnerGrade()
    {
        if (_runnerGrade <= 0)
            return null;

        if (_runnerData._runnerGrade.Count < _runnerGrade + 1)
            return null;

        return _runnerData._runnerGrade[_runnerGrade];
    }

    public void RunnerLevelUp()
    {
        _runnerGrade = Mathf.Min(_runnerGrade + 1, _runnerData._runnerGrade.Count);
        _modelControl.ActiveModel(_runnerGrade);        
    }
}
