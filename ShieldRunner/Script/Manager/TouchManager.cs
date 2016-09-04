using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(EventSystem))]
[RequireComponent(typeof(StandaloneInputModule))]

public class TouchManager : Singleton<TouchManager>, ITitleLoadAsynk
{	
	TouchPhase _touchState = TouchPhase.Ended;
	TouchPhase _touchStateLast = TouchPhase.Ended;
	
	Vector3 _touchOriginPos = Vector3.zero;

	[SerializeField]
	float _checkDragDistance = 1.0f;

	EventSystem _eventSystem = null;

	/* Event */
	
	public Action<Vector2> _touchBeganEvent = null;
	public Action<Vector2> _touchEndedEvent = null;
	public Action<Vector2> _touchClickedEvent = null;
	public Action<Vector2, Vector2, float> _touchMovedEvent = null;
	public Action<Vector2, Vector2, float> _touchMoveEndedEvent = null;
	
    // Method

	void Awake()
	{
		_eventSystem = EventSystem.current;
        _eventSystem.firstSelectedGameObject = gameObject;
        _eventSystem.sendNavigationEvents = false;

		_touchBeganEvent += this.TouchBegan;
		_touchEndedEvent += this.TouchEnded;
		_touchClickedEvent += this.TouchClicked;
		_touchMovedEvent += this.TouchMoved;
		_touchMoveEndedEvent += this.TouchMoveEnded;
	}

	void OnDestroy()
	{
		_touchBeganEvent = null;
		_touchEndedEvent = null;
		_touchClickedEvent = null;
		_touchMovedEvent = null;
		_touchMoveEndedEvent = null;
	}
	
	void Update()
	{
        #if UNITY_STANDALONE || UNITY_WEBPLAYER
        UpdateMouseInput();
        #endif

        #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        UpdateTouchInput(false);
        #endif 
	}

    #region ITitleLoadAsynk

    public void InitAsynk()
    {
        CreateInstance();
    }

    #endregion

    // 마우스 처리
    #if UNITY_STANDALONE || UNITY_WEBPLAYER
    void UpdateMouseInput()
    {
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
    }
    #endif

    // 터치 및 관련 포지션 정보 획득 처리
    #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

    void UpdateTouchInput(bool isPosCheck)
    {
        if (Input.touchCount <= 0)
            return;

        Touch touch = Input.touches[0];

        if (touch.phase == TouchPhase.Began)
        {
            if (IsPointerOverUI(touch.position) == true)
                return;
        }

        Vector2 touchPos = Vector2.zero;
        if (isPosCheck == true)
        {
            touchPos = GetTouchWorldPos(touch.position);;
        }

        switch (touch.phase)
        {
            case TouchPhase.Began:
                {
                    SetTouchState(TouchPhase.Began, touchPos);
                }
                break;

            case TouchPhase.Moved:
                {
                    SetTouchState(TouchPhase.Moved, touchPos);
                }
                break;

            case TouchPhase.Ended:
                {
                    SetTouchState(TouchPhase.Ended, touchPos);
                }
                break;

            default:
                break;
        }
    }

    #endif

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

        for (int index = 0; index < raycastResults.Count; ++index)
		{
            RaycastResult result = raycastResults[index];
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
			_touchBeganEvent(touchPos);

			_touchOriginPos = touchPos;
		}
			break;

		case TouchPhase.Moved:
		{
			float distance = Vector3.Distance(touchPos, _touchOriginPos);
			_touchMovedEvent(_touchOriginPos, touchPos, distance);
		}
			break;

		case TouchPhase.Ended:
		{
			float distance = Vector3.Distance(touchPos, _touchOriginPos);

			if (_touchStateLast == TouchPhase.Began)
			{
				if (distance < _checkDragDistance)
				{
					_touchClickedEvent(touchPos);
				}
			}
			else if (_touchStateLast == TouchPhase.Moved)
			{
				_touchMoveEndedEvent(_touchOriginPos, touchPos, distance);
			}

			_touchEndedEvent(touchPos);
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

	public Vector2 GetTouchDirection(Vector2 originPos, Vector2 endPos)
	{
		Vector2 direction = endPos - originPos;
		float angle = Vector2.Angle(Vector2.up, direction);

		if (angle <= 45.0f)
		{
			return Vector2.up;
		}
		else if (angle >= 135.0f)
		{
			return Vector2.down;
		}
		else
		{
			if (direction.x > 0.0f)
			{
				return Vector2.right;
			}
			else if (direction.x < 0.0f)
			{
				return Vector2.left;
			}
		}

		return Vector2.zero;
	}
}
