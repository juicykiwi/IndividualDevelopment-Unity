using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class MainSceneControl : SceneControl<MainSceneControl>
{
    [SerializeField]
    UIMainScene_PlayerInfoView _playerInfoView = null;

    [SerializeField]
    UIMainScene_UpgradeShield _upgradeShield = null;

    [SerializeField]
    UIMainScene_EnchantShield _enchantShield = null;

    [SerializeField]
    UIMainScene_ViewAds _viewAds = null;

    protected override SceneControlType SceneControlType
    {
        get { return SceneControlType.Main; }
    }

    // Method

	protected override void Start()
	{
        base.Start();

        UpdateMainSceneUI();

        AdMobManager.instance.Init();
	}

	public void OnStartButton()
	{
        int gameTryCount = Mathf.Max(0, PlayerDataManager.instance.GameTryCount) + 1;
        PlayerDataManager.instance.GameTryCount = gameTryCount;

        /* AdMob Interstitial */
        if (AdMobManager.instance.TestMode == false)
        {
            AdMobManager.instance.InitInterstitial();
        }

		LoadLevelScene(SceneHelper.StageSceneName);
	}

    public void UpdateMainSceneUI()
    {
        _playerInfoView.UpdateInfoView();

        _upgradeShield.CheckButtonActive();
        _enchantShield.CheckButtonActive();
        _viewAds.CheckButtonActive();
    }
}
