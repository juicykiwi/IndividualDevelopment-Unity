using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class StageSceneControl : SceneControl<StageSceneControl>
{
    const int DivideValueForAds = 4;

    [SerializeField]
    int PreEnqueueTile01Count = 200;

    [SerializeField]
    bool _testMode = false;
    public bool TestMode { get { return _testMode; } }

	bool _isStageStart = false;

	// Stage conrol

	[SerializeField]
	StageControl _stageControl = null;
	public StageControl StageControl { get { return _stageControl; } }

	// Stage battle object conrol

	[SerializeField]
	StageBattleObjectControl _stageBattleObjectControl = null;
	public StageBattleObjectControl StageBattleObjectControl
	{
		get
		{
			if (_stageBattleObjectControl == null)
			{
				_stageBattleObjectControl = GameObjectHelper.NewGameObject<StageBattleObjectControl>();
				_stageBattleObjectControl.transform.SetParent(transform);
			}
			
			return _stageBattleObjectControl;
		}
	}

    // 저 사양 기기에서 프레임 저하가 심해 셰이더 처리 사용하지 않음
//	[SerializeField]
//    StageBackgroundScrollerByShader _backgroundScroller = null;

    [SerializeField]
    StageBackgroundScroller _backgroundScroller = null;

    [SerializeField]
    UIStageScene_PlayerInfoView _playerInfoView = null;

    protected override SceneControlType SceneControlType
    {
        get { return SceneControlType.Stage; }
    }

    // Method

	#region MonoBehavior event

	protected override void Start()
	{
        base.Start();

		StartStage();
	}

	void Update()
	{
		UpdateKey();
	}

	void FixedUpdate()
	{
		if (_isStageStart == true)
		{
            UpdateInfoView();

			if (IsStageFail() == true)
			{
				StopStage();
                return;
			}
		}
	}

	#endregion

    protected override void Clear()
    {
        ClearStage();
    }

	void StartStage()
	{
        System.GC.Collect();

        TileManager.instance.PreSettingPool(1, PreEnqueueTile01Count);
        MonsterManager.instance.PreSettingPool(1001, 30);
        MonsterManager.instance.PreSettingPool(1002, 30);
        MonsterManager.instance.PreSettingPool(1003, 30);
        PickupItemManager.instance.PreSettingPool(1, 10);

		// Create stage conrol and init

		_stageControl = GameObjectHelper.NewGameObject<StageControl>();
		_stageControl.transform.SetParent(transform);

		_stageControl.Init();

		// Touch
		TouchManager.instance._touchBeganEvent += OnStageTouchBegan;
		TouchManager.instance._touchEndedEvent += OnStageTouchEnded;
		
		// Hero setting

        HeroObject heroObject = HeroManager.instance.HeroFactory.CreateHeroObject(HeroManager.HeroObjectId, true);
        if (heroObject != null)
        {
            HeroManager.instance.AddCreatedHeroInDict(heroObject);

            heroObject.SelectHero();
            heroObject.ModelControl.SetPos(new Vector2(0f, 3f));

            int baseShieldId = heroObject.InfoData._baseEquipShieldId;
            int lastShieldId = PlayerDataManager.instance.PlayerData._lastEquipedShieldId;
            int enchantCount = PlayerDataManager.instance.GetEnchantCount();

            heroObject.ModelControl.EquipItemSlotShield.EquipShield(baseShieldId, lastShieldId);
            heroObject.ModelControl.EquipItemSlotShield.IncreaseEquipItemDurability(enchantCount);
        }
		
		// Follow camera for hero
		CameraManager.instance.ChangedCameraPositionEvent += _backgroundScroller.OnChangedCameroPosition;
		CameraManager.instance.SetFollowTarget(heroObject.gameObject, new Vector2(0.8f, 0.95f));

		// Start stage control
		_stageControl.StartStage();

		_isStageStart = true;
	}

	public void StopStage()
	{
        float lastMoveDistance = HeroManager.instance.SelectHeroObejct.CurrentMoveDistance();
        if (IsNewBestMoveDistance(lastMoveDistance) == true)
        {
            UpdateNewBestMoveDistance(lastMoveDistance);
        }

        PlayerDataManager.instance.SaveData();

		CameraManager.instance.ClearFollowTarget();

		_stageControl.StopStage();

		Invoke("ReStartStage", 3f);

		_isStageStart = false;
	}

	public void ClearStage()
	{
		TouchManager.instance._touchBeganEvent -= OnStageTouchBegan;
		TouchManager.instance._touchEndedEvent -= OnStageTouchEnded;

		BlockManager.instance.RemoveBlockObjectAll();

		HeroManager.instance.RemoveCreatedHeroDictAll();
        MonsterManager.instance.Clear();

        PickupItemManager.instance.Clear();
        TileManager.instance.Clear();

		CameraManager.instance.ChangedCameraPositionEvent -= _backgroundScroller.OnChangedCameroPosition;
	}

	public void ReStartStage()
	{
        int gameTryCount = PlayerDataManager.instance.GameTryCount;
        if (gameTryCount > 0 && gameTryCount % DivideValueForAds == 0)
        {
            if (AdMobManager.instance.TestMode == false)
            {
                AdMobManager.instance.ShowInterstitial();
            }
        }

		LoadLevelScene(SceneHelper.MainSceneName);
	}

	public bool IsStageFail()
	{
		HeroObject hero = HeroManager.instance.SelectHeroObejct;
		if (hero == null)
			return true;

		if (CameraManager.instance.IsNeedDestroyByObjectPos (hero.transform.position) == true)
			return true;

		return false;
	}
	
	#region Best move distance

	bool IsNewBestMoveDistance(float currentDistance)
	{
		float bestMoveDistance = PlayerDataManager.instance.BestMoveDistance();
		return (bestMoveDistance < currentDistance);
	}

	void UpdateNewBestMoveDistance(float currentDistance)
	{
		PlayerDataManager.instance.SaveNewBestMoveDistance(currentDistance);
	}

	#endregion

    #region UI

    void UpdateInfoView()
    {
        if (HeroManager.instance.SelectHeroObejct == null)
            return;
        
        float heroMoveDistance = HeroManager.instance.SelectHeroObejct.CurrentMoveDistance();
        _playerInfoView.UpdateInfoView(heroMoveDistance);
    }

    #endregion

    #region Touch control

	public void OnStageTouchBegan(Vector2 touchPos)
	{
		HeroObject heroObejct = HeroManager.instance.SelectHeroObejct;
		heroObejct.SetMoveUpCommand(true);
	}

	public void OnStageTouchEnded(Vector2 touchPos)
	{
		HeroObject heroObejct = HeroManager.instance.SelectHeroObejct;
		heroObejct.SetMoveUpCommand(false);
	}

    #endregion

    #region Key control

	void UpdateKey()
	{
        #if UNITY_EDITOR

		if (Input.GetKeyDown(KeyCode.Space) == true)
		{
			HeroObject heroObejct = HeroManager.instance.SelectHeroObejct;
			heroObejct.SetMoveUpCommand(true);
		}
		else if (Input.GetKeyUp(KeyCode.Space) == true)
		{
			HeroObject heroObejct = HeroManager.instance.SelectHeroObejct;
			heroObejct.SetMoveUpCommand(false);
		}

        #endif
	}

    #endregion
}
