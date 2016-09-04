using UnityEngine;
using System.Collections;

public class RunningMachine : MonoBehaviour
{
    [SerializeField]
    RunningMachineRailControl _railControl = null;

    Runner _runner = null;
    TrainingMenu _trainingMenu = null;

    float _speed = 0f;
    public float Speed { get { return _speed; } }

    float _lastOrderTime = 0f;

    void Awake()
    {
        enabled = false;
    }

    void Update()
    {
        if (_trainingMenu == null)
        {
            return;
        }

        RunningMachineSpeedProcess();
    }

    public void Init(Runner runner)
    {
        _runner = runner;
    }

    public void StartRunningMachine()
    {
        enabled = true;
    }

    public void StopRunningMachine()
    {
        enabled = false;
        SetSpeed(0f, true);
        _runner.StopRunner();
    }

    public void SetTrainingMenu(TrainingMenu trainingMenu)
    {
        _lastOrderTime = 0f;
        _trainingMenu = trainingMenu;
    }

    public void RunningMachineSpeedProcess()
    {
        _lastOrderTime += Time.deltaTime;

        float velocityDiff = _trainingMenu.Velocity() - _speed;
        if (velocityDiff == 0f)
        {
            return;
        }

        float speed = 0f;
        float accelTimeDiff = _trainingMenu.AccelForce() - _lastOrderTime;
        if (accelTimeDiff <= 0)
        {
            speed = _trainingMenu.Velocity();
        }
        else
        {
            speed = _speed + velocityDiff * Time.deltaTime;
        }

        SetSpeed(speed, false);
    }

    public void SetSpeed(float speed, bool force)
    {
        _speed = speed;
        _railControl.SetRailSpeed(_speed);
        _runner.SetRunnerMachineSpeed(_speed, force);
    }
}
