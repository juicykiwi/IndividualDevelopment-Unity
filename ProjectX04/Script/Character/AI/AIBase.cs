using UnityEngine;
using System.Collections;

public enum AIState
{
	None,
	Spawn,
	Idle,
	IdleForUser,
	Detect,
	Attack,
	Move,
	Die,
	Max,
}

public class AIBase : MonoBehaviour {

	// Property

	protected AIState _state = AIState.None;
	public AIState state { get { return _state; } }

	protected AIController _aiController = null;
	public AIController aiController
	{
		set { _aiController = value; }
		get { return _aiController; }
	}

	protected bool _isFirstUpdate = true;
	protected float _runTime = 0.0f;
	protected float _fixedRunTime = 0.0f;

	public AIMessage _aiMessage = null;

	// Method

	protected virtual void Awake () {
		this.enabled = false;
	}

	// Use this for initialization
	protected virtual void Start () {

	}
	
	// Update is called once per frame
	protected virtual void Update () {
		_runTime += Time.deltaTime;

		if (_isFirstUpdate == true)
		{
			FirstUpdate();
			
			_isFirstUpdate = false;
		}
	}

	protected virtual void FixedUpdate () {
		_fixedRunTime += Time.fixedDeltaTime;
	}

	protected virtual void FirstUpdate()
	{
	}

	protected virtual void OnEnable () {
		_isFirstUpdate = true;

		_runTime = 0.0f;
		_fixedRunTime = 0.0f;
	}

	protected virtual void OnDisable () {
		_aiMessage = null;
	}

	public virtual bool IsPossibleNextAIState(AIState aiState)
	{
		return true;
	}
}
