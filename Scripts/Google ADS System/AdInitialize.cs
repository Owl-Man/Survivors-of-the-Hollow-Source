using GoogleMobileAds.Api;
using UnityEngine;

namespace Google_ADS_System
{
    public class AdInitialize : MonoBehaviour
    {
        private void Awake()
        {
            MobileAds.Initialize(initStatus => { });
            DontDestroyOnLoad(gameObject);
        }
    }
}