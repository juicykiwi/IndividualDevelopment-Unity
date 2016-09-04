using UnityEngine;
using System.Collections;

public class VerticalMoveControl : MonoBehaviour
{
    public enum MoveType
    {
        None,
        Up,
        Down,
    }

    [SerializeField]
    BattleObject _battleObject = null;

    [SerializeField]
    ModelControl _modelControl = null;

    [SerializeField]
    bool _isStarted = false;

    [SerializeField]
    float _totalDeltaTime = 0f;

    // Method

    #region Monobehavior event

    void Update()
    {
        if (_isStarted == true)
        {
            if (IsStopMove() == true)
            {
                StopMove();
                return;
            }
        }

        if (IsStartMove() == false)
        {
            return;
        }

        StartMove();

        if (IsStartNow() == true)
        {
            do
            {
                HeroObject heroObject = _battleObject as HeroObject;
                if (heroObject != null)
                {
                    if (heroObject.MoveUpCommand == true)
                        break;
                }

                _totalDeltaTime = _battleObject.BattleStat._currentJumpTime * 0.5f;

            } while (false);
        }

        Move();
    }

    #endregion

    #region Start / Stop control

    void StartMove()
    {
        if (_isStarted == false)
        {
            _isStarted = true;
            _totalDeltaTime = 0;
        }
    }

    void StopMove()
    {
        _isStarted = false;
    }

    bool IsStartNow()
    {
        return (_totalDeltaTime == 0f);
    }

    bool IsMoveUpState()
    {
        return (_totalDeltaTime < _battleObject.BattleStat._currentJumpTime * 0.5f);
    }

    bool IsStartMove()
    {
        if (_modelControl.IsGround() == true)
        {
            HeroObject heroObject = _battleObject as HeroObject;
            if (heroObject != null)
            {
                if (heroObject.MoveUpCommand == true)
                {
                    return true;
                }
            }

            return false;
        }

        return true;
    }

    bool IsStopMove()
    {
        if (IsMoveUpState() == true)
            return false;
        
        if (_modelControl.IsGround() == false)
            return false;

        return true;
    }

    #endregion

    void Move()
    {
        float prePosY = CurrentMovePos();

        _totalDeltaTime += Time.deltaTime;
        float currentPosY = CurrentMovePos();

        float deltaPos = currentPosY - prePosY;
        deltaPos = (int)(deltaPos * 100f) * 0.01f;
        _battleObject.ModelControl.MoveDown(deltaPos);
    }

    float CurrentMovePos()
    {
        float currentTime = _totalDeltaTime;
        float endTime = _battleObject.BattleStat._currentJumpTime;

        if (currentTime >= endTime)
        {
            float overPosValue = OverPosValue(currentTime - endTime);
            return overPosValue * _battleObject.BattleStat._currentJumpHeight;
        }

        float degree = currentTime / endTime * 180f;
        float sinValue = GameObjectHelper.SinByDegree(degree);

        return sinValue * _battleObject.BattleStat._currentJumpHeight;
    }

    float OverPosValue(float overTime)
    {
        return overTime * -5f;
    }
}
