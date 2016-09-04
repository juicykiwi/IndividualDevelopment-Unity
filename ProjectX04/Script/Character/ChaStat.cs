using UnityEngine;
using System.Collections;

public class ChaStat : MonoBehaviour {

	// Field & Property

	public int _maxLife = 3;

	public int _life = 0;

	public int _detectRange = 0;

	public float _moveSpeed = 0.0f;
	public float _attackSpeed = 0.0f;
	public float _attackRange = 1.0f;
	public float _idleWaitTime = 0.0f;

	// Method

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool IsDie()
	{
		return (_life <= 0);
	}

	public bool IsFullCurrentHp()
	{
		return (_life >= _maxLife);
	}
}
