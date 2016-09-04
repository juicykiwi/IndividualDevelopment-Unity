using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(EventSystem))]
[RequireComponent(typeof(StandaloneInputModule))]

public class TouchManager : Singleton<TouchManager>
{	
	TouchPhase _touchState = TouchPhase.Ended;
	TouchPhase _touchStateLast = TouchPhase.Ended;
	
	Vector3 _touchOriginPos = Vector3.zero;

	[SerializeField]
	float _checkDragDistance = 1.0f;

	/* Event */
	
    // parameter : screen touch pos
	public Action<Vector2> _touchBeganEvent = null;

    // parameter : screen touch pos
    public Action<Vector2> _touchEndedEvent = null;

    // parameter : screen touch pos
    public Action<Vector2> _touchClickedEvent = null;

    // parameter : screen origin touch pos, screen last touch Pos, distance
    public Action<Vector2, Vector2, float> _touchMovedEvent = null;

    // parameter : screen origin touch pos, screen last touch Pos, distance
    public Action<Vector2, Vector2, float> _touchMoveEndedEvent = null;
	
	/**/

    void Start()
    {
        EventSystem.current.sendNavigationEvents = false;
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
        UpdateTouchInput();
        #endif 
	}

    // 마우스 처리
    #if UNITY_STANDALONE || UNITY_WEBPLAYER
    void UpdateMouseInput()
    {
        if (Input.GetMouseButton(0) == true)
        {
            if (IsPointerOverUI(Input.mousePosition) == true)
                return;

            switch (_touchState)
            {
                case TouchPhase.Ended:
                    {
                        SetTouchState(TouchPhase.Began, Input.mousePosition);
                    }
                    break;

                case TouchPhase.Began:
                    {
                        if (Vector3.Distance(Input.mousePosition, _touchOriginPos) >= _checkDragDistance)
                        {
                            SetTouchState(TouchPhase.Moved, Input.mousePosition);
                        }
                    }
                    break;

                default:
                    break;
            }
        }
        else if (Input.GetMouseButton(0) == false)
        {
            switch (_touchState)
            {
                case TouchPhase.Began:
                    {
                        SetTouchState(TouchPhase.Ended, Input.mousePosition);
                    }
                    break;

                case TouchPhase.Moved:
                    {
                        SetTouchState(TouchPhase.Ended, Input.mousePosition);
                    }
                    break;
            }
        }
    }
    #endif

    // 터치 및 관련 포지션 정보 획득 처리
    #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8

    void UpdateTouchInput()
    {
        if (Input.touchCount <= 0)
            return;

        Touch touch = Input.touches[0];

        if (touch.phase == TouchPhase.Began)
        {
            if (IsPointerOverUI(touch.position) == true)
                return;
        }

        switch (touch.phase)
        {
            case TouchPhase.Began:
                {
                    SetTouchState(TouchPhase.Began, touch.position);
                }
                break;

            case TouchPhase.Moved:
                {
                    SetTouchState(TouchPhase.Moved, touch.position);
                }
                break;

            case TouchPhase.Ended:
                {
                    SetTouchState(TouchPhase.Ended, touch.position);
                }
                break;

            default:
                break;
        }
    }

    #endif

	bool IsPointerOverUI(Vector2 touchPos)
	{
        if (EventSystem.current == null)
			return false;

        PointerEventData pointer = new PointerEventData(EventSystem.current);
		pointer.position = touchPos;
		
		List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);
		
		if (raycastResults.Count <= 0)
			return false;

        for (int index = 0; index < raycastResults.Count; ++index)
		{
            RaycastResult result = raycastResults[index];
			if (result.gameObject == null)
				continue;
			
            if (result.gameObject.layer != LayerMask.NameToLayer("UI"))
                continue;

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
                    if (_touchBeganEvent != null)
                    {
                        _touchBeganEvent(touchPos);
                    }

                    _touchOriginPos = touchPos;
                }
                break;
		
            case TouchPhase.Moved:
                {
                    float distance = Vector3.Distance(touchPos, _touchOriginPos);
                    if (_touchMovedEvent != null)
                    {
                        _touchMovedEvent(_touchOriginPos, touchPos, distance);
                    }
                }
                break;

            case TouchPhase.Ended:
                {
                    float distance = Vector3.Distance(touchPos, _touchOriginPos);

                    switch (_touchStateLast)
                    {
                        case TouchPhase.Began:
                            {
                                if (distance < _checkDragDistance)
                                {
                                    if (_touchClickedEvent != null)
                                    {
                                        _touchClickedEvent(touchPos);
                                    }
                                }
                            }
                            break;

                        case TouchPhase.Moved:
                            {
                                if (_touchMoveEndedEvent != null)
                                {
                                    _touchMoveEndedEvent(_touchOriginPos, touchPos, distance);
                                }
                            }
                            break;

                        default:
                            break;
                    }

                    if (_touchEndedEvent != null)
                    {
                        _touchEndedEvent(touchPos);
                    }
                }
                break;

            default:
                break;
		}
	}

	static public Vector3 GetTouchWorldPos(Vector3 touchPos)
	{
		return Camera.main.ScreenToWorldPoint(touchPos);
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
