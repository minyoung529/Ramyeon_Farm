using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    private InterstitialAd interstitial;

    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            RequestInterstitial();
        });
    }

    private void RequestInterstitial()
    {
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";

        interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }
    
    public void AdsShow()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        else
        {
            RequestInterstitial();
        }
    }

    public bool IsAdLoad()
    {
        return interstitial.IsLoaded();
    }
}
