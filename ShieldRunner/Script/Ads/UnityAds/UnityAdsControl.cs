using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class UnityAdsControl : MonoBehaviour
{
	[SerializeField]
	public string _iosGameId = "";

	[SerializeField]
	public string _androidGameId = "";

	[SerializeField]
	bool _enableTestMode = true;


	void Start()
	{
		string gameId = "";

        #if UNITY_IOS
        gameId = _iosGameId;
		#endif

		#if UNITY_ANDROID
		gameId = _androidGameId;
		#endif

        if (string.IsNullOrEmpty(gameId) == true)
        {
            Debug.Log("Failed to initialize Unity Ads. Game ID is null or empty.");
            return;
        }

        if (Advertisement.isSupported == false)
        {
            Debug.Log("Unable to initialize Unity Ads. Platform not supported.");
            return;
        }

        if (Advertisement.isInitialized == true)
        {
            Debug.Log("Unity Ads is already initialized.");
            return;
        }
            
        Advertisement.Initialize(gameId, _enableTestMode);
	}
}
