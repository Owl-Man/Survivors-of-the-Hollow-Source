using GoogleMobileAds.Api;
using UnityEngine;

namespace Google_ADS_System
{
    public class RewardAd : MonoBehaviour
    {
        private string _rewardUnitId = "ca-app-pub-3940256099942544/5224354917";

        [SerializeField] private bool isADKeyTest;
        
        public static RewardAd Instance;
        
        private RewardedAd _rewardedAd;

        private void Awake() 
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            if (!isADKeyTest) _rewardUnitId = "ca-app-pub-4275611682046066/5736834105";
            Instance = this;
        }

        private void Start() => LoadRewardedAd();

        public void ShowRewardedAd(string rewardType)
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                _rewardedAd.Show((Reward reward) =>
                {
                    switch (rewardType)
                    {
                        case "gold":
                            ResourcesManager.Instance.ChangeGoldValue(5);
                            break;
                        case "raspberyl":
                            ResourcesManager.Instance.ChangeRaspberylValue(3);
                            break;
                        case "sapphire":
                            ResourcesManager.Instance.ChangeSapphireValue(1);
                            break;
                    }
                });
            }
        }

        
        public void LoadRewardedAd()
        {
            // Clean up the old ad before loading a new one.
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();
            adRequest.Keywords.Add("unity-admob-sample");

            // send the request to load the ad.
            RewardedAd.Load(_rewardUnitId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    _rewardedAd = ad;
                });
            
            RegisterEventHandlers(_rewardedAd);
        }
        
        
        private void RegisterEventHandlers(RewardedAd ad)
        {
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded Ad full screen content closed.");

                // Reload the ad so that we can show another as soon as possible.
                LoadRewardedAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);

                // Reload the ad so that we can show another as soon as possible.
                LoadRewardedAd();
            };

        }
    }
}