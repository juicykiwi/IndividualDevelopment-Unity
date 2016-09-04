using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;  // 반드시 추가

public class AdMobManager : Singleton<AdMobManager>
{
    [SerializeField]
    bool _testMode = false;
    public bool TestMode { get { return _testMode; } }

    [SerializeField]
    string _bannerAdMobId = "";
    public string BannerAdMobId { get { return _bannerAdMobId; } }

    [SerializeField]
    string _interstitialAdMobIdAndroid = "";
    public string InterstitialAdMobIdAndroid { get { return _interstitialAdMobIdAndroid; } }

    [SerializeField]
    string _interstitialAdMobIdIOS = "";
    public string InterstitialAdMobIdIOS { get { return _interstitialAdMobIdIOS; } }

    // BannerView 현재 사용하지 않음
//    BannerView bannerView;

    InterstitialAd interstitial;

    string _interstitialId = "";
    public string InterstitialId { get { return _interstitialId; } }

    bool _isAlreadyInit = false;

    // Method

    public void Init()
    {
        if (_isAlreadyInit == true)
            return;
        
        #if UNITY_ANDROID

        // Use this for initialization

        _interstitialId = _interstitialAdMobIdAndroid;

        #elif UNITY_IOS

        _interstitialId = _interstitialAdMobIdIOS;

        #endif

        _isAlreadyInit = true;
    }


    #region Banner

//    public void InitBanner()
//    {
//        #if UNITY_IOS || UNITY_ANDROID
//
//        // Create an empty ad request.
//        AdRequest request = new AdRequest.Builder().Build();
//
//        bannerView = new BannerView(_bannerAdMobId, AdSize.Banner, AdPosition.Top);
//        // Load the Ads with the request.
//        bannerView.LoadAd(request);
//
//        #endif
//    }

//    public void ShowBanner()
//    {
//        #if UNITY_IOS || UNITY_ANDROID
//
//        // 배너 광고 Show
//        bannerView.Show();
//
//        #endif
//    }

    #endregion


    #region Interstitial

    public void InitInterstitial()
    {
        #if UNITY_IOS || UNITY_ANDROID

        interstitial = new InterstitialAd(_interstitialId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the Ads with the request.
        interstitial.LoadAd(request);

        #endif
    }

    public bool IsLoadedInterstitial()
    {
        #if UNITY_IOS || UNITY_ANDROID

        return interstitial.IsLoaded();

        #else

        return false;

        #endif
    }

    public void ShowInterstitial()
    {
        #if UNITY_IOS || UNITY_ANDROID

        if (interstitial.IsLoaded())
        {
            interstitial.Show();  // 전면 광고 Show
        }

        #endif
    }

    #endregion
}