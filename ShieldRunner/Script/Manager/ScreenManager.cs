using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DeviceResolutionType
{
	None = -1,
	Iphone_5s_Portrait,
	Iphone_5s_Landscape,
}

public class ScreenManager : Singleton<ScreenManager>, ITitleLoadAsynk
{
	[SerializeField]
	DeviceResolutionType _defaultDeviceResolution = DeviceResolutionType.Iphone_5s_Landscape;
	public DeviceResolutionType DefaultDeviceResolution { get { return _defaultDeviceResolution; } }

	Dictionary<int, Vector2> _deviceResolution = new Dictionary<int, Vector2>()
	{
		{ (int)DeviceResolutionType.None, 					new Vector2(1024, 768) },
		{ (int)DeviceResolutionType.Iphone_5s_Portrait, 	new Vector2(640, 1136) },
		{ (int)DeviceResolutionType.Iphone_5s_Landscape, 	new Vector2(1136, 640) },
	};

	[SerializeField]
	Vector2 _screenSize = Vector2.zero;
	public Vector2 ScreenSize { get { return _screenSize; } }

	[SerializeField]
	Vector2 _screenToWorldSize = Vector2.zero;
	public Vector2 ScreenToWorldSize { get { return _screenToWorldSize; } }

    // Method

	void Awake()
	{
		_screenSize = new Vector2(Screen.width, Screen.height);
	}

	void Start()
	{
		Vector3 worldPointMin = CameraManager.instance.GetScreenToWorldPoint(Vector2.zero);
		Vector3 worldPointMax = CameraManager.instance.GetScreenToWorldPoint(_screenSize);
		
		_screenToWorldSize = new Vector2(
			worldPointMax.x - worldPointMin.x,
			worldPointMax.y - worldPointMin.y);
	}

    #region ITitleLoadAsynk

    public void InitAsynk()
    {
        CreateInstance();
    }

    #endregion

	public Vector2 GetDeviceResolution(DeviceResolutionType type)
	{
		if (_deviceResolution.ContainsKey((int)type) == false)
			return Vector2.zero;

		return _deviceResolution[(int)type];
	}
}
