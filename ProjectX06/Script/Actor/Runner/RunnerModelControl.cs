using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunnerModelControl : MonoBehaviour
{
    [SerializeField]
    List<RunnerAnimatorControl> _animatorControlList = new List<RunnerAnimatorControl>();

    RunnerAnimatorControl _currentAnimatorConrol = null;


    void Awake()
    {
        for (int index = 0; index < _animatorControlList.Count; ++index)
        {
            if (_animatorControlList[index] == null)
                continue;

            _animatorControlList[index].gameObject.SetActive(false);
        }
    }

    public void ActiveModel(int runnerGrade)
    {
        if (runnerGrade <= 0)
        {
            Debug.LogWarning("Invalid runnerGrade value.");
            return;
        }
        else if (runnerGrade > _animatorControlList.Count)
        {
            Debug.LogWarning("Failed ActiveModel(). RunnerGrade value over list.");
            return;
        }
        else if (_currentAnimatorConrol == _animatorControlList[runnerGrade - 1])
        {
            return;
        }

        if (_currentAnimatorConrol != null)
        {
            _currentAnimatorConrol.gameObject.SetActive(false);
        }

        _currentAnimatorConrol = _animatorControlList[runnerGrade - 1];
        _currentAnimatorConrol.gameObject.SetActive(true);
    }

    public void SetAnimation(RunnerAnimateType animateType)
    {
        if (_currentAnimatorConrol == null)
            return;

        _currentAnimatorConrol.SetState(animateType);
    }
}
