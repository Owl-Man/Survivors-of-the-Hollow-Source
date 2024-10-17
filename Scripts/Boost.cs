using Localization;
using UnityEngine;
using UnityEngine.UI;

public class Boost : MonoBehaviour
{
    public string boostType;
    public ushort id;
    public string description;
    public Button button;
    public GameObject outline, availabilityIcon;

    public int goldCost, raspberylCost, sapphireCost;

    private void Awake()
    {
        if (id != 0) button.interactable = false;
    }

    private void Start()
    {
        LocalizationManager.Instance.OnLanguageChanged += UpdateDescription;
        UpdateDescription();
    }
    
    public void UpdateDescription() => description = LocalizationManager.Instance.GetLocalizedValue(boostType);
}
