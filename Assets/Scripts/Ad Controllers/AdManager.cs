using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    public GameObject AdOptions;
    public GameObject AdError;
    public GameObject AdButton;

    private string PlayStoreID = "3602693";
    private string AppStoreID = "3602692";
    private string InterstitialAd = "video";
    private string RewardedAd = "rewardedVideo";

    public bool IsTargetAppStore; // is game going to the app store or play store
    public bool IsTestAd;
    private bool AdAvailable;
    private bool MusicStateBeforeAd;

    void Start()
    {
        // initialize the ad
        AdButton.GetComponent<Button>().onClick.AddListener(PlayRewardAd);
        InitializeAd();
        AdAvailable = true;
        AdOptions.SetActive(true);
        AdError.SetActive(false);
    }

    void InitializeAd()
    {
        if (IsTargetAppStore) {
            Advertisement.Initialize(AppStoreID, IsTestAd);
        } else {
            Advertisement.Initialize(PlayStoreID, IsTestAd);
        }
    }

    public void PlayRewardAd()
    {
      print("reward");
        if (Advertisement.IsReady(RewardedAd) && AdAvailable) {
            // get the state of the music before playing the ad
            int enabled = PlayerPrefs.GetInt("MusicEnabled", 0);
            MusicStateBeforeAd = (enabled == 1);
            AdAvailable = false;
            AdOptions.SetActive(false);
            Advertisement.AddListener(this);
            Advertisement.Show(RewardedAd);
        }
    }

    private void BroadcastMusicState()
    {
        if (MusicStateBeforeAd) {
            GameEvents.current.MusicUnMuted();
        } else {
            GameEvents.current.MusicMuted();
        }
    }

    // unity ads listeners
    public void OnUnityAdsReady (string placementId) {
        // Advertisement.Show (placementId);
    }
    public void OnUnityAdsDidError (string errorMessage) {
        BroadcastMusicState();
        GameEvents.current.AdErrored();
        AdError.SetActive(true);
        Advertisement.RemoveListener(this);
    }
    public void OnUnityAdsDidStart (string placementId) {
        // mute audio when ad starts
        GameEvents.current.AdStarted();
        GameEvents.current.MusicMuted();
    }
    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {
        BroadcastMusicState();
        if (showResult == ShowResult.Finished) {
            GameEvents.current.AdFinished();
        } else if (showResult == ShowResult.Skipped) {
            // nothing
        } else if (showResult == ShowResult.Failed) {
            GameEvents.current.AdErrored();
            AdError.SetActive(true);
        } else {
            print("error");
        }
        Advertisement.RemoveListener(this);
    }

}
