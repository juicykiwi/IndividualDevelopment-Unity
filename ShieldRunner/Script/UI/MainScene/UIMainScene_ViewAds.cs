using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;

public class UIMainScene_ViewAds : MonoBehaviour
{
    const int GetGoldViewAd = 100;

    [SerializeField]
    Button _button = null;

    [SerializeField]
    Text _getGoldText = null;

    bool _isAlreadyReword = false;

    // Method

    void Awake()
    {
        _isAlreadyReword = false;
    }

    void FixedUpdate()
    {
        CheckButtonActive();
    }

    public void CheckButtonActive()
    {
        bool isActive = true;
        bool isInteractable = true;
        string goldText = "-";

        do
        {
            goldText = GetGoldViewAd.ToString();

            #if (! UNITY_EDITOR) && (UNITY_IOS || UNITY_ANDROID)

            if (Advertisement.isSupported == false || Advertisement.isInitialized == false)
            {
                isInteractable = false;
                break;
            }

            #endif

            if (_isAlreadyReword == true)
            {
                isInteractable = false;
                break;
            }

        } while (false);

        if (isActive == true)
        {
            _button.interactable = isInteractable;
            _getGoldText.text = goldText;
        }

        gameObject.SetActive(isActive);
    }
        
    public void OnUnityAdsButton()
    {
        #if UNITY_EDITOR

        PlayerDataManager.instance.IncreaseGold(GetGoldViewAd, true);
        _isAlreadyReword = true;

        MainSceneControl.instance.UpdateMainSceneUI();

        #elif UNITY_IOS || UNITY_ANDROID

        if (Advertisement.IsReady() == true)
        {
            ShowOptions showOption = new ShowOptions();
            showOption.resultCallback = HandleShowResult;

            Advertisement.Show("rewardedVideo", showOption);
        }

        #endif
    }

    private void HandleShowResult(ShowResult result)
    {
        bool isReward = false;

        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Video completed. Offer a reward to the player.");
                isReward = true;
                break;

            case ShowResult.Skipped:
                Debug.LogWarning("Video was skipped.");
                isReward = true;
                break;

            case ShowResult.Failed:
                Debug.LogError("Video failed to show.");
                break;
        }

        if (isReward == false)
            return;

        _isAlreadyReword = true;

        PlayerDataManager.instance.IncreaseGold(GetGoldViewAd, true);

        MainSceneControl.instance.UpdateMainSceneUI();
    }
}
