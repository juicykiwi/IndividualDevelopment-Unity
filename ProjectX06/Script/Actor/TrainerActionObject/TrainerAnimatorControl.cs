using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TrainerAnimateType
{
    None,
    Idle,
    SelectL,
    SelectR,
}

public class TrainerAnimatorControl : MonoBehaviour
{
    [SerializeField]
    Animator _animator = null;

    TrainerAnimateType _currentType = TrainerAnimateType.None;

    Dictionary<TrainerAnimateType, string> _animateKeyDict = new Dictionary<TrainerAnimateType, string>();


    void Awake()
    {
        _animateKeyDict.Add(TrainerAnimateType.None, "Idle");
        _animateKeyDict.Add(TrainerAnimateType.Idle, "Idle");
        _animateKeyDict.Add(TrainerAnimateType.SelectL, "SelectL");
        _animateKeyDict.Add(TrainerAnimateType.SelectR, "SelectR");
    }

    public void SetState(TrainerAnimateType type)
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

    public void SelectLAnimation(int count, float repeatingTime)
    {
        StartCoroutine(SelectLAnimationCoroutine(count, repeatingTime));
    }

    IEnumerator SelectLAnimationCoroutine(int count, float repeatingTime)
    {
        while (count > 0)
        {
            --count;

            SetState(TrainerAnimateType.SelectL);
            yield return new WaitForSeconds(repeatingTime);

            SetState(TrainerAnimateType.Idle);
            yield return new WaitForSeconds(repeatingTime);
        }

        yield break;
    }

    public void SelectRAnimation(int count, float repeatingTime)
    {        
        StartCoroutine(SelectRAnimationCoroutine(count, repeatingTime));
    }

    IEnumerator SelectRAnimationCoroutine(int count, float repeatingTime)
    {
        while (count > 0)
        {
            --count;

            SetState(TrainerAnimateType.SelectR);
            yield return new WaitForSeconds(repeatingTime);

            SetState(TrainerAnimateType.Idle);
            yield return new WaitForSeconds(repeatingTime);
        }

        yield break;
    }
}
