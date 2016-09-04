using UnityEngine;
using System;
using System.Collections;

public class CameraManager : Singleton<CameraManager>, ITitleLoadAsynk
{
	[SerializeField]
	public Camera MainCamera { get { return Camera.main; } }

	[SerializeField]
	GameObject _followTarget = null;

	[SerializeField]
	Vector3 _offsetForFollowTarget = Vector3.zero;

	[SerializeField]
	float _destroyOffsetPosX = 5f;

	[SerializeField]
	float _destroyOffsetPosY = 5f;

    // 해상도 처리 관련

    [SerializeField]
    float _defaultOrthographicSize = 5.0f;

    [SerializeField]
    bool _isFullScreen = false;

    [SerializeField]
    int _defaultScreenWidth = 1136;

    [SerializeField]
    int _defaultScreenHeight = 640;

    [SerializeField]
    public Rect _screenRectInWorld = new Rect();

	public Action<Vector3> ChangedCameraPositionEvent = null;

    // Method

	void LateUpdate()
	{
		if (_followTarget != null)
		{
			Vector3 targerPos = _followTarget.transform.position;
			Vector3 cameraPos = new Vector3(
				targerPos.x + _offsetForFollowTarget.x,
				_offsetForFollowTarget.y,
				Camera.main.transform.position.z);

			Camera.main.transform.position = cameraPos;

			if (ChangedCameraPositionEvent != null)
			{
				ChangedCameraPositionEvent(cameraPos);
			}
		}
	}

    #region ITitleLoadAsynk

    public void InitAsynk()
    {
        CreateInstance();
        Init();
    }

    #endregion

    public void Init()
    {
        MainCameraResolutionUpdate();
    }

	public Vector3 GetScreenToWorldPoint(Vector2 screenPoint)
	{
		return Camera.main.ScreenToWorldPoint(screenPoint);
	}

	public void SetFollowTarget(GameObject target, Vector2 viewport)
	{
		_followTarget = target;

		if (_followTarget == null)
			return;

		Vector3 viewportWorldPoint = Camera.main.ViewportToWorldPoint(viewport);
		Vector3 viewportWorldPointCenter = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));

		_offsetForFollowTarget = viewportWorldPoint - viewportWorldPointCenter;
	}

	public void ClearFollowTarget()
	{
		_followTarget = null;
	}

	public bool IsNeedDestroyByObjectPos(Vector2 objectPos)
	{
		if (SceneHelper.IsStageScene() == false)
		{
			return false;
		}

		Vector3 screenMinPoint = GetScreenToWorldPoint(Vector2.zero);
		if (objectPos.x <= screenMinPoint.x - _destroyOffsetPosX)
			return true;

		if (objectPos.y <= screenMinPoint.y - _destroyOffsetPosY)
			return true;

		return false;
	}
        
    #region 해상도 관련 처리

    public void MainCameraResolutionUpdate()
    {
        SetResolution();

        SetOrthographicSize(_defaultOrthographicSize);
    }

    void SetResolution()
    {
        if (Camera.main == null)
            return;
        
        Camera.main.aspect = (float)_defaultScreenWidth / (float)_defaultScreenHeight; // 1.775

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        CaculateSceenWorldRect(new Vector2(screenWidth, screenHeight));

        float ratioDefaultWidth = (float)screenWidth / (float)_defaultScreenWidth;
        float ratioDefaultHeight = (float)screenHeight / (float)_defaultScreenHeight;

        float currendScreenAspect = (float)screenWidth / (float)screenHeight;

        if (_isFullScreen == false)
        {
            if (Camera.main.aspect >= currendScreenAspect)
            {
                // Width 중심으로 Height 세팅
                float ratioCurrentHeight = ratioDefaultWidth / ratioDefaultHeight;
                Camera.main.rect = new Rect (0f, (1f - ratioCurrentHeight) / 2f, 1f, ratioCurrentHeight);
            }
            else
            {
                // Height 중심으로 Width 세팅
                float ratioCurrentWidth = ratioDefaultHeight / ratioDefaultWidth;
                Camera.main.rect = new Rect ((1f - ratioCurrentWidth) / 2f, 0f, ratioCurrentWidth, 1f);
            }
        }
    }

    void CaculateSceenWorldRect(Vector2 screenSize)
    {
        Vector3 worldPointMin = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector3 worldPointMax = Camera.main.ScreenToWorldPoint(new Vector3(screenSize.x, screenSize.y, 0f));

        _screenRectInWorld.xMin = worldPointMin.x;
        _screenRectInWorld.yMin = worldPointMin.y;

        _screenRectInWorld.xMax = worldPointMax.x;
        _screenRectInWorld.yMax = worldPointMax.y;
    }

    void SetOrthographicSize(float size)
    {
        if (Camera.main == null)
            return;
        
        Camera.main.orthographicSize = size;
    }

    #endregion
}
