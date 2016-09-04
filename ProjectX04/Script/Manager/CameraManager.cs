using UnityEngine;
using System.Collections;

public class CameraManager : ManagerBase<CameraManager> {

	/* Init setting info
	 * Landscape
	 * - DefaultOrthographicSize : 5
	 * - _defaultScreenWidth : 1136
	 * - _defaultScreenHeight : 640
	 * 
	 * Portrait
	 * - DefaultOrthographicSize : 8.0
	 * - _defaultScreenWidth : 640
	 * - _defaultScreenHeight : 1136
	 */

	public const float DefaultOrthographicSize = 8.0f;
	public const string MainCameraPrefapPath = "Camera/";
	public const string MainCameraPrefapName = "Main Camera";

	public int _defaultScreenWidth = 640;
	public int _defaultScreenHeight = 1136;
	public bool _isFullScreen = false;

	public Vector2 _screenSize = Vector2.zero;
	public Rect _screenRectInWorld = new Rect();
	public Rect _enableMoveRect = new Rect();

	public Camera _main = null;

	// Method

	protected override void Awake () {
		base.Awake();

		MakeMainCamera();
		
		/* OrthorgraphicResize()보다 SetResolution() 방식이
		 * 시스템에 맏다고 생각하여 사용
		 * 작업이 마무리 되었을 시 어는 것이 맞는지 또는 수정이 필요한지 확인 필요
		 */ 
		
		SetResolution();

		SetOrthographicSize(DefaultOrthographicSize);
	}

	// Use this for initialization
	void Start () {

	}

	public void MakeMainCamera()
	{
		if (_main)
			return;

		GameObject MainCameraPrefab = Resources.Load(
			MainCameraPrefapPath + MainCameraPrefapName) as GameObject;

		if (MainCameraPrefab == null)
			return;

		GameObject MainCameraObj = Instantiate(MainCameraPrefab) as GameObject;
		if (MainCameraObj == null)
			return;

		MainCameraObj.name = MainCameraPrefapName;
		MainCameraObj.transform.SetParent(transform);
		_main = MainCameraObj.GetComponent<Camera>();
	}
	
	public override void ActionSceneLoaded(SceneType sceneType)
	{
		SceneType[] typeArray = new SceneType[] {
			SceneType.FieldMaker,
			SceneType.SelectStage,
			SceneType.GameStage
		};

		bool isActive = IsHaveSceneTypeFormArray(sceneType, typeArray);
		_main.gameObject.SetActive(isActive);
	}
	
//	public override void ActionSceneClosed(SceneType sceneType)

	public void SetResolutionPrev()
	{
		// 기본 설정으로 카메라 화면비 설정 및 출력 Rect 변경

		int screenWidth = Screen.width;
		int screenHeight = Screen.height;

		float ratioDefaultWidth = (float)screenWidth / (float)_defaultScreenWidth;
		float ratioDefaultHeight = (float)screenHeight / (float)_defaultScreenHeight;

		float ratioCurrentHeight = ratioDefaultWidth / ratioDefaultHeight;
		
		_main.aspect = (float)_defaultScreenWidth / (float)_defaultScreenHeight;
		_main.rect = new Rect (0f, (1f - ratioCurrentHeight) / 2f, 1f, ratioCurrentHeight);
	}

	public void SetResolution()
	{
		// 기본 설정으로 카메라 화면비 설정 및 출력 Rect 변경
		
		// Set Aspect
		
		_main.aspect = (float)_defaultScreenWidth / (float)_defaultScreenHeight; // 1.775
		
		// Set Viewport rect
		
		int screenWidth = Screen.width;
		int screenHeight = Screen.height;

		CaculateSceenWorldRect();

		float ratioDefaultWidth = (float)screenWidth / (float)_defaultScreenWidth;
		float ratioDefaultHeight = (float)screenHeight / (float)_defaultScreenHeight;
		
		float currendScreenAspect = (float)screenWidth / (float)screenHeight; // 1.3333
		
		if (_isFullScreen == false)
		{
			if (_main.aspect >= currendScreenAspect)
			{
				// Width 중심으로 Height 세팅
				float ratioCurrentHeight = ratioDefaultWidth / ratioDefaultHeight;
				_main.rect = new Rect (0f, (1f - ratioCurrentHeight) / 2f, 1f, ratioCurrentHeight);
			}
			else
			{
				// Height 중심으로 Width 세팅
				float ratioCurrentWidth = ratioDefaultHeight / ratioDefaultWidth;
				_main.rect = new Rect ((1f - ratioCurrentWidth) / 2f, 0f, ratioCurrentWidth, 1f);
			}
		}
	}

	public void SetOrthographicSize(float size)
	{
		_main.orthographicSize = size;
	}

	public void OrthographicResize()
	{
		// 카메라의 OrthographicSize를 변경하는 방식

		/* 해상도 및 화면비
		 * 
		 * < iOS >
		 * iphone5s : 1136:640 = 16:9
		 * 4s : 960:640 = 3:2
		 * 6 : 1334:750 = 16:9
		 * 6 Plus : 2208:1242 = 16:9
		 * iPad2 : 1024:768 = 4:3
		 * 
		 * < Android >
		 * Nexus 7-1 : 1280:800 = 16:10
		 */

		const float DefaultScreenHeight = 1136f;
		const float DefaultScreenWidth = 640;

		// 9:16 세로 가로 비율 = 0.5633f
		float rateWidthHeight = (float)DefaultScreenHeight / (float)DefaultScreenWidth;
		
		int screenWidth = Screen.width;
		int screenHeight = Screen.height;

		Debug.LogFormat("Screen width:{0}, height:{1}", screenWidth, screenHeight);

		float rateWidth = (float)screenWidth / (float)DefaultScreenWidth;
		float rateHeight = (float)screenHeight / (float)DefaultScreenHeight;

		if (rateWidth <= rateHeight)
		{
			float newScreenHeight = screenWidth * rateWidthHeight;	// 현재 스크린 비율에 대한 높이 값
			float rateOldNewHeight = newScreenHeight / screenHeight;
			
			_main.orthographicSize = CameraManager.DefaultOrthographicSize / rateOldNewHeight;
		}
		else
		{	
			float newScreenWidth = screenHeight / rateWidthHeight;
			float rateOldNewWidth = newScreenWidth / screenWidth;
			
			_main.orthographicSize = CameraManager.DefaultOrthographicSize / rateOldNewWidth;
		}
	}

	void CaculateSceenWorldRect()
	{
		_screenSize.x = Screen.width;
		_screenSize.y = Screen.height;

		Vector3 worldPointMin = _main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
		Vector3 worldPointMax = _main.ScreenToWorldPoint(new Vector3(_screenSize.x, _screenSize.y, 0f));

		_screenRectInWorld.xMin = worldPointMin.x;
		_screenRectInWorld.yMin = worldPointMin.y;

		_screenRectInWorld.xMax = worldPointMax.x;
		_screenRectInWorld.yMax = worldPointMax.y;
	}

	public void SetEnableMoveRect(Rect fieldRect)
	{
		_enableMoveRect.width = fieldRect.width - _screenRectInWorld.width;
		_enableMoveRect.height = fieldRect.height - _screenRectInWorld.height;
		_enableMoveRect.center = fieldRect.center;
	}

	public void MoveForMoveRectCenter()
	{
		_main.transform.position = new Vector3(_enableMoveRect.center.x,
		                                       _enableMoveRect.center.y,
		                                       _main.transform.position.z);

	}

	public void Move(Direction direction)
	{
		Vector2 directionVector = GameHelper.GetVectorWithDirection(direction);
		Move(directionVector);
	}

	public void Move(Vector3 destPos)
	{
		_main.transform.position += destPos; 
	}

	public void SetPosition(Vector2 pos)
	{
		_main.transform.position = new Vector3(pos.x, pos.y, _main.transform.position.z);
	}

	public struct MoveForTargetValue
	{
		public GameObject _target;
		public Vector2 _oringPos;
		public Vector2 _destPos;
	};

	#region MoveForTarget

	public void MoveStartForTarget(GameObject target, Vector2 originPos, Vector3 destPos)
	{
		MoveForTargetValue moveValue = new MoveForTargetValue ()
		{
			_target = target,
			_oringPos = originPos,
			_destPos = destPos,
		};

		StartCoroutine("MoveForTarget", moveValue);
	}

	public void MoveEndForTarget()
	{
		StopCoroutine("MoveForTarget");
	}

	IEnumerator MoveForTarget(MoveForTargetValue moveForTargetValue)
	{
		Transform targetTrans = moveForTargetValue._target.transform;
		Vector2 originPos = moveForTargetValue._oringPos;
		Vector2 destPos = moveForTargetValue._destPos;

		Vector3 movePos = _main.transform.position;

		while(true)
		{
			movePos.x = moveForTargetValue._target.transform.position.x;
			movePos.y = moveForTargetValue._target.transform.position.y;

			_main.transform.position = movePos;

			yield return null;
		}
	}

	#endregion
}
