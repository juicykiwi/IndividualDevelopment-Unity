using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class BattleStat : ICloneable
{
	public int _baseHp;
	public float _baseSpeed;
	public float _baseJumpTime;
	public float _baseJumpHeight;

	public int _currentHp;
	public float _currentSpeed;
	public float _currentJumpTime;
	public float _currentJumpHeight;

    public bool IsDead { get { return (_currentHp <= 0); } }

    // Method

    public void InitCurrentStat()
    {
        _currentHp = _baseHp;
        _currentSpeed = _baseSpeed;
        _currentJumpTime = _baseJumpTime;
        _currentJumpHeight = _baseJumpHeight;
    }

    #region ICloneable

    public object Clone()
    {
        BattleStat clone = new BattleStat()
            {
                _baseHp = this._baseHp,
                _baseSpeed = this._baseSpeed,
                _baseJumpTime = this._baseJumpTime,
                _baseJumpHeight = this._baseJumpHeight,

                _currentHp = this._currentHp,
                _currentSpeed = this._currentSpeed,
                _currentJumpTime = this._currentJumpTime,
                _currentJumpHeight = this._currentJumpHeight
            };

        return clone;
    }

    #endregion
}
