using UnityEngine;
using System;
using System.Collections;

public class CameraResolutionManager : Singleton<CameraResolutionManager>
{
    /* 해상도 처리 관련 */

    [SerializeField]
    float _defaultOrthographicSize = 1.0f;

    [SerializeField]
    bool _isFullScreen = false;

    [SerializeField]
    int _defaultScreenWidth = 640;

    [SerializeField]
    int _defaultScreenHeight = 1136;

    [SerializeField]
    public Rect _screenRectInWorld = new Rect();


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
        if (Camera.main == null)
            return;
        
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

    public Vector3 GetScreenToWorldPoint(Vector2 screenPoint)
    {
        if (Camera.main == null)
            return Vector3.zero;

        return Camera.main.ScreenToWorldPoint(screenPoint);
    }
}
