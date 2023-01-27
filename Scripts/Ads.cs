using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class Ads : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private string gameId = "5107427", type = "Interstitial_Android";
    private bool testMode = true;
    private EnemySpawner spawner;

    private bool adsCompleted = false;
    private IEnumerator ShowAd;
    private void Start() 
    {
        spawner = FindObjectOfType<EnemySpawner>();
        ShowAd = ShowAdCourotine();
        Advertisement.Initialize(gameId, testMode, null);
    }

    private void Update() 
    {
        if(Advertisement.isInitialized && spawner.waveNum % 2 == 0 && !adsCompleted)
        {
            StartCoroutine(ShowAd);
            Advertisement.Show(type, this);
            OnUnityAdsShowComplete(type, UnityAdsShowCompletionState.SKIPPED);
            OnUnityAdsShowComplete(type, UnityAdsShowCompletionState.COMPLETED);
        }
        if(spawner.waveNum % 2 != 0)
        adsCompleted = false;
        
    }

    IEnumerator ShowAdCourotine()
    {
        Advertisement.Load(type);
        yield return new WaitForSeconds(5f);
    }

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string type)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
    }
 
    public void OnUnityAdsFailedToLoad(string type, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {type} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }
 
    public void OnUnityAdsShowFailure(string type, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {type}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }
 
    public void OnUnityAdsShowStart(string type) 
    { 

    }
    public void OnUnityAdsShowClick(string type) 
    { 

    }
    public void OnUnityAdsShowComplete(string type, UnityAdsShowCompletionState showCompletionState) 
    { 
        adsCompleted = true;
    }
    public interface IUnityAdsLoadListener 
    {
        void OnUnityAdsAdLoaded(string adUnitId);
        void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message);
    }
    public interface IUnityAdsShowListener 
    {
        void OnUnityAdsShowFailure(string type, UnityAdsShowError error, string message);
        void OnUnityAdsShowStart(string type);
        void OnUnityAdsShowClick(string type);
        void OnUnityAdsShowComplete(string type, UnityAdsShowCompletionState showCompletionState);
    }

}