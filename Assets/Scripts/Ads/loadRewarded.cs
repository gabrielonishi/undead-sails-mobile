using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class loadRewarded : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private int GAME_SCENE_BUILD_INDEX = 6;
    public string androidAdUnitId;
    public string iosAdUnitId;
    
    string adUnitId;

    PlayerInventory inventory;

    void Awake()
    {
#if UNITY_IOS
        adUnitId = iosAdUnitId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#elif UNITY_EDITOR
        adUnitId = androidAdUnitId;
#endif
    }

    void Start()
    {
        inventory = PlayerInventory.Instance;
    }

    public void LoadAd()
    {
        print("Loading Rewarded!!");
        Advertisement.Load(adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId.Equals(adUnitId))
        {
            print("Rewarded loaded!!");
            ShowAd();
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        print("Rewarded failed to load");
    }



    public void ShowAd()
    {
        print("showing Rewarded ad!!");
        Advertisement.Show(adUnitId, this);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        print("Rewarded clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        print("Rewarded show complete");
        SceneManager.LoadScene(GAME_SCENE_BUILD_INDEX);
        inventory.watchedAd();
        if (placementId.Equals(adUnitId)&&showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
        {
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        print("Rewarded show failure");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        print("Rewarded show start");

    }
}