using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{


#if UNITY_ANDROID
        private string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif
    private InterstitialAd _interstitial;
    private int _nowLosed;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        DestroyAndStartNew(true);
    }

    private void Update()
    {
        if (_interstitial.IsLoaded() && GameController.countLoses % 3 == 0 && GameController.countLoses != 0 && GameController.countLoses != _nowLosed)
        {
            _nowLosed = GameController.countLoses;
            _interstitial.Show();
        }
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        DestroyAndStartNew();
    }
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        DestroyAndStartNew();
    }
    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        DestroyAndStartNew();
    }

    void DestroyAndStartNew(bool isFirts = false)
    {
        if (!isFirts)
            _interstitial.Destroy();

        _interstitial = new InterstitialAd(adUnitId);
        // Called when an ad request failed to load.
        _interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when the ad is closed.
        _interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        _interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        _interstitial.LoadAd(request);
    }

}
