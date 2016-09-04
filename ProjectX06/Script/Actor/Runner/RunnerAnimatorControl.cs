using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum RunnerAnimateType
{
    None,
    Spawn,
    Idle,
    Run,
}

public class RunnerAnimatorControl : MonoBehaviour
{
    [SerializeField]
    Animator _animator = null;

    RunnerAnimateType _currentType = RunnerAnimateType.None;

    Dictionary<RunnerAnimateType, string> _animateKeyDict = new Dictionary<RunnerAnimateType, string>();


    void Awake()
    {
        _animateKeyDict.Add(RunnerAnimateType.None, "Idle");
        _animateKeyDict.Add(RunnerAnimateType.Spawn, "Idle");
        _animateKeyDict.Add(RunnerAnimateType.Idle, "Idle");
        _animateKeyDict.Add(RunnerAnimateType.Run, "Run");
    }

    public void SetState(RunnerAnimateType type)
    {
        if (type == _currentType)
            return;

        if (_animateKeyDict.ContainsKey(_currentType) == true)
        {
            _animator.SetBool(_animateKeyDict[_currentType], false);
        }

        _currentType = type;
        if (_animateKeyDict.ContainsKey(_currentType) == true)
        {
            _animator.SetBool(_animateKeyDict[_currentType], true);
        }
    }
}
