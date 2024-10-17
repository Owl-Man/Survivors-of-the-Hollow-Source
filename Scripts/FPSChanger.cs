using System;
using TMPro;
using UnityEngine;

public class FPSChanger : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsView;

    private void Start() => UpdateValue();

    public void OnFPSChangeClick()
    {
        if (FPSController.Instance.GetFPS() == 60) FPSController.Instance.SetFPS(90);
        else if (FPSController.Instance.GetFPS() == 90) FPSController.Instance.SetFPS(120);
        else if (FPSController.Instance.GetFPS() == 120) FPSController.Instance.SetFPS(144);
        else FPSController.Instance.SetFPS(60);

        UpdateValue();
    }

    public void OnSyncButtonClick()
    {
        FPSController.Instance.SetFPS((int)Screen.currentResolution.refreshRateRatio.value);
        UpdateValue();
    }
    
    public void UpdateValue() => fpsView.text = FPSController.Instance.GetFPS().ToString();
}
