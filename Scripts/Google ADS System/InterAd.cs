using GoogleMobileAds.Api;
using UnityEngine;

namespace Google_ADS_System
{
    public class InterAd : MonoBehaviour
    {
        private InterstitialAd _interstitialAd;

        [SerializeField] private bool isADKeyTest;

        private string _interstitialUnitId = "ca-app-pub-3940256099942544/1033173712";

        public static InterAd Instance;

        private void Awake() 
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            if (!isADKeyTest) _interstitialUnitId = "ca-app-pub-4275611682046066/7438294594";
            Instance = this;
        }

        private void Start() => LoadAd();

        public void ShowAd()
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd()) _interstitialAd.Show();
        }

        public void LoadAd()
        {
            // Clean up the old ad before loading a new one.
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }

            Debug.Log("Loading the interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();
            adRequest.Keywords.Add("unity-admob-sample");

            // send the request to load the ad.
            InterstitialAd.Load(_interstitialUnitId, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("interstitial ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());

                    _interstitialAd = ad;
                });
            
            RegisterEventHandlers(_interstitialAd);
        }
        
        private void RegisterEventHandlers(InterstitialAd ad)
        {
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial Ad full screen content closed.");

                // Reload the ad so that we can show another as soon as possible.
                LoadAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Interstitial ad failed to open full screen content " +
                               "with error : " + error);

                // Reload the ad so that we can show another as soon as possible.
                LoadAd();
            };

        }

    }
}