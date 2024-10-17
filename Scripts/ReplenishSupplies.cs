using Google_ADS_System;
using UnityEngine;

public class ReplenishSupplies : MonoBehaviour
{
    public void OnOpenClick()
    {
        Time.timeScale = 0f;
    }

    public void OnCloseClick()
    {
        Time.timeScale = 1f;
    }

    public void OnGoldAdClick() => RewardAd.Instance.ShowRewardedAd("gold");
    public void OnRaspberylAdClick() => RewardAd.Instance.ShowRewardedAd("raspberyl");
    public void OnSapphireAdClick() => RewardAd.Instance.ShowRewardedAd("sapphire");
}