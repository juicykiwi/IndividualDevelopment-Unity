using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class TouchManager : ManagerBase<TouchManager> {
	
	TouchPhase _touchState = TouchPhase.Ended;
	TouchPhase _touchStateLast = TouchPhase.Ended;
	
	Vector3 _touchOriginPos = Vector3.zero;
	const float _checkDragDistance = 1.0f;

	EventSystem _eventSystem = null;

	// Delegate
	
	public Action<Vector2> _actionTouchBegan = null;
	public Action<Vector2> _actionTouchEnded = null;
	public Action<Vector2> _actioTouchClicked = null;
	public Action<Vector2, Vector2, float> _actionTouchMoved = null;
	public Action<Vector2, Vector2, float> _actionTouchMoveEnded = null;

	// Method

	protected override void Awake () {
		base.Awake();

		gameObject.AddComponent<EventSystem>();
		gameObject.AddComponent<StandaloneInputModule>();
		gameObject.AddComponent<TouchInputModule>();

		_eventSystem = EventSystem.current;
	}

	// Use this for initialization
	void Start () {
		_actionTouchBegan += this.TouchBegan;
		_actionTouchEnded += this.TouchEnded;
		_actioTouchClicked += this.TouchClicked;
		_actionTouchMoved += this.TouchMoved;
		_actionTouchMoveEnded += this.TouchMoveEnded;
	}

	void OnDestroy () {
		_actionTouchBegan -= this.TouchBegan;
		_actionTouchEnded -= this.TouchEnded;
		_actioTouchClicked -= this.TouchClicked;
		_actionTouchMoved -= this.TouchMoved;
		_actionTouchMoveEnded -= this.TouchMoveEnded;
	}

	// Update is called once per frame
	void Update () {
//#if UNITY_EDITOR
#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if (Input.GetMouseButton(0) == true)
		{
			if (IsPointerOverUI(Input.mousePosition) == true)
				return;

			if (_touchState == TouchPhase.Ended)
			{
				Vector2 touchPos = GetTouchWorldPos(Input.mousePosition);
				SetTouchState(TouchPhase.Began, touchPos);
			}
			else if (_touchState == TouchPhase.Began)
			{
				Vector2 touchPos = GetTouchWorldPos(Input.mousePosition);
				if (Vector3.Distance(touchPos, _touchOriginPos) >= _checkDragDistance)
				{
					SetTouchState(TouchPhase.Moved, touchPos);
				}
			}
		}

		if (Input.GetMouseButton(0) == false)
		{
			if (_touchState == TouchPhase.Began)
			{
				Vector2 touchPos = GetTouchWorldPos(Input.mousePosition);
				SetTouchState(TouchPhase.Ended, touchPos);
			}
			else if (_touchState == TouchPhase.Moved)
			{
				Vector2 touchPos = GetTouchWorldPos(Input.mousePosition);
				SetTouchState(TouchPhase.Ended, touchPos);
			}
		}
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
		if (Input.touchCount <= 0)
			return;
		
		Touch touch = Input.touches[0];

		if (IsPointerOverUI(touch.position) == true)
			return;
		
		if (touch.phase == TouchPhase.Began)
		{
			Vector2 touchPos = GetTouchWorldPos(touch.position);
			SetTouchState(TouchPhase.Began, touchPos);
		}
		else if (touch.phase == TouchPhase.Moved)
		{
			Vector2 touchPos = GetTouchWorldPos(touch.position);
			SetTouchState(TouchPhase.Moved, touchPos);
		}
		else if (touch.phase == TouchPhase.Ended)
		{
			Vector2 touchPos = GetTouchWorldPos(touch.position);
			SetTouchState(TouchPhase.Ended, touchPos);
		}
#endif 
	}

//	public override void ActionSceneLoaded(SceneType sceneType)
//	public override void ActionSceneClosed(SceneType sceneType)

	bool IsPointerOverUI(Vector2 touchPos)
	{
		if (_eventSystem == null)
			return false;

		PointerEventData pointer = new PointerEventData(_eventSystem);
		pointer.position = touchPos;
		
		List<RaycastResult> raycastResults = new List<RaycastResult>();
		_eventSystem.RaycastAll(pointer, raycastResults);
		
		if (raycastResults.Count <= 0)
			return false;

		foreach (RaycastResult result in raycastResults)
		{
			if (result.gameObject == null)
				continue;
			
			if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
				return true;
		}

		return false;
	}

	void SetTouchState(TouchPhase touchPhase, Vector2 touchPos)
	{
		_touchStateLast = _touchState;
		_touchState = touchPhase;

		switch (_touchState)
		{
		case TouchPhase.Began:
		{
			_actionTouchBegan(touchPos);

			_touchOriginPos = touchPos;
		}
			break;

		case TouchPhase.Moved:
		{
			float distance = Vector3.Distance(touchPos, _touchOriginPos);
			_actionTouchMoved(_touchOriginPos, touchPos, distance);
		}
			break;

		case TouchPhase.Ended:
		{
			float distance = Vector3.Distance(touchPos, _touchOriginPos);

			if (_touchStateLast == TouchPhase.Began)
			{
				if (distance < _checkDragDistance)
				{
					_actioTouchClicked(touchPos);
				}
			}
			else if (_touchStateLast == TouchPhase.Moved)
			{
				_actionTouchMoveEnded(_touchOriginPos, touchPos, distance);
			}
			else
			{
				_actionTouchEnded(touchPos);
			}
		}
			break;

		default:
			break;
		}
	}

	public void TouchBegan(Vector2 touchPos)
	{

	}

	public void TouchEnded(Vector2 touchPos)
	{

	}

	public void TouchClicked(Vector2 touchPos)
	{
		
	}

	public void TouchMoved(Vector2 originPos, Vector2 endPos, float distance)
	{
		
	}

	public void TouchMoveEnded(Vector2 originPos, Vector2 endPos, float distance)
	{
		
	}

	Vector2 GetTouchWorldPos(Vector3 touchPos)
	{
		Vector3 worldPos3D = Camera.main.ScreenToWorldPoint(touchPos);
		return new Vector2(worldPos3D.x, worldPos3D.y);
	}

	public Direction GetTouchDirection(Vector2 originPos, Vector2 endPos)
	{
		Vector2 direction = endPos - originPos;
		float angle = Vector2.Angle(Vector2.up, direction);

		if (angle <= 45.0f)
		{
			return Direction.Up;
		}
		else if (angle >= 135.0f)
		{
			return Direction.Down;
		}
		else
		{
			if (direction.x > 0.0f)
			{
				return Direction.Right;
			}
			else if (direction.x < 0.0f)
			{
				return Direction.Left;
			}
		}

		return Direction.None;
	}
}
