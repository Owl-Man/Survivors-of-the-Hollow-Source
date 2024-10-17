using GunSystem;
using Localization;
using TMPro;
using UnityEngine;

public class Improvements : MonoBehaviour
{
    [SerializeField] private GameObject panel, anyImpAvailabilityNotify;
    
    [SerializeField] private TMP_Text description;
    private string _defaultDescription;
    
    [SerializeField] private float boostValue;
    
    [SerializeField] private AudioSource lockedSound;

    public float CurrentSpeedBoost { get; private set; }

    [SerializeField] private Boost[] shelterHpBoosts, speedBoosts, drillBoosts,
        bulletPowerBoosts, gunRotateSpeedBoosts, gunReloadingSpeedBoosts, jetpackLoadCapacityBoosts;

    private Boost _boostCache;
    private ResourcesManager _rm;

    public static Improvements Instance;

    private void Awake() => Instance = this;

    private void Start()
    {
        _rm = ResourcesManager.Instance;
        _defaultDescription = LocalizationManager.Instance.GetLocalizedValue("CHOOSE IMPROVEMENT");
    }

    public void OnOpenButtonClick()
    {
        panel.SetActive(true);
        Time.timeScale = 0;
        CheckBoostsAvailability();
    }

    public void OnCloseButtonClick()
    {
        Time.timeScale = 1f;
        _rm.UpdateValues();
        description.text = _defaultDescription;
        _boostCache = null;
    }

    public void CheckAnyBoostsAvailability()
    {
        for (int i = 0; i < shelterHpBoosts.Length; i++)
        {
            if (shelterHpBoosts[i].button.enabled && shelterHpBoosts[i].button.interactable && EnoughResources(shelterHpBoosts[i]))
            {
                anyImpAvailabilityNotify.SetActive(true);
                return;
            }
        }
        
        for (int i = 0; i < speedBoosts.Length; i++)
        {
            if (speedBoosts[i].button.enabled && speedBoosts[i].button.interactable && EnoughResources(speedBoosts[i]))
            {
                anyImpAvailabilityNotify.SetActive(true);
                return;
            }
        }
        
        for (int i = 0; i < drillBoosts.Length; i++)
        {
            if (drillBoosts[i].button.enabled && drillBoosts[i].button.interactable && EnoughResources(drillBoosts[i]))
            {
                anyImpAvailabilityNotify.SetActive(true);
                return;
            }
        }
        
        for (int i = 0; i < bulletPowerBoosts.Length; i++)
        {
            if (bulletPowerBoosts[i].button.enabled && bulletPowerBoosts[i].button.interactable && EnoughResources(bulletPowerBoosts[i]))
            {
                anyImpAvailabilityNotify.SetActive(true);
                return;
            }
        }
        
        for (int i = 0; i < gunRotateSpeedBoosts.Length; i++)
        {
            if (gunRotateSpeedBoosts[i].button.enabled && gunRotateSpeedBoosts[i].button.interactable && EnoughResources(gunRotateSpeedBoosts[i]))
            {
                anyImpAvailabilityNotify.SetActive(true);
                return;
            }
        }
        
        for (int i = 0; i < gunReloadingSpeedBoosts.Length; i++)
        {
            if (gunReloadingSpeedBoosts[i].button.enabled && gunReloadingSpeedBoosts[i].button.interactable && EnoughResources(gunReloadingSpeedBoosts[i]))
            {
                anyImpAvailabilityNotify.SetActive(true);
                return;
            }
        }

        for (int i = 0; i < jetpackLoadCapacityBoosts.Length; i++)
        {
            if (jetpackLoadCapacityBoosts[i].button.enabled && jetpackLoadCapacityBoosts[i].button.interactable && EnoughResources(jetpackLoadCapacityBoosts[i]))
            {
                anyImpAvailabilityNotify.SetActive(true);
                return;
            }
        }
        
        anyImpAvailabilityNotify.SetActive(false);
    }

    private void CheckBoostsAvailability()
    {
        for (int i = 0; i < shelterHpBoosts.Length; i++)
        {
            if (shelterHpBoosts[i].button.enabled && shelterHpBoosts[i].button.interactable && EnoughResources(shelterHpBoosts[i]))
            {
                shelterHpBoosts[i].availabilityIcon.SetActive(true);
            }
            else shelterHpBoosts[i].availabilityIcon.SetActive(false);
        }
        
        for (int i = 0; i < speedBoosts.Length; i++)
        {
            if (speedBoosts[i].button.enabled && speedBoosts[i].button.interactable && EnoughResources(speedBoosts[i]))
            {
                speedBoosts[i].availabilityIcon.SetActive(true);
            }
            else speedBoosts[i].availabilityIcon.SetActive(false);
        }
        
        for (int i = 0; i < drillBoosts.Length; i++)
        {
            if (drillBoosts[i].button.enabled && drillBoosts[i].button.interactable && EnoughResources(drillBoosts[i]))
            {
                drillBoosts[i].availabilityIcon.SetActive(true);
            }
            else drillBoosts[i].availabilityIcon.SetActive(false);
        }
        
        for (int i = 0; i < bulletPowerBoosts.Length; i++)
        {
            if (bulletPowerBoosts[i].button.enabled && bulletPowerBoosts[i].button.interactable && EnoughResources(bulletPowerBoosts[i]))
            {
                bulletPowerBoosts[i].availabilityIcon.SetActive(true);
            }
            else bulletPowerBoosts[i].availabilityIcon.SetActive(false);
        }
        
        for (int i = 0; i < gunRotateSpeedBoosts.Length; i++)
        {
            if (gunRotateSpeedBoosts[i].button.enabled && gunRotateSpeedBoosts[i].button.interactable && EnoughResources(gunRotateSpeedBoosts[i]))
            {
                gunRotateSpeedBoosts[i].availabilityIcon.SetActive(true);
            }
            else gunRotateSpeedBoosts[i].availabilityIcon.SetActive(false);
        }
        
        for (int i = 0; i < gunReloadingSpeedBoosts.Length; i++)
        {
            if (gunReloadingSpeedBoosts[i].button.enabled && gunReloadingSpeedBoosts[i].button.interactable && EnoughResources(gunReloadingSpeedBoosts[i]))
            {
                gunReloadingSpeedBoosts[i].availabilityIcon.SetActive(true);
            }
            else gunReloadingSpeedBoosts[i].availabilityIcon.SetActive(false);
        }
        
        for (int i = 0; i < jetpackLoadCapacityBoosts.Length; i++)
        {
            if (jetpackLoadCapacityBoosts[i].button.enabled && jetpackLoadCapacityBoosts[i].button.interactable && EnoughResources(jetpackLoadCapacityBoosts[i]))
            {
                jetpackLoadCapacityBoosts[i].availabilityIcon.SetActive(true);
            }
            else jetpackLoadCapacityBoosts[i].availabilityIcon.SetActive(false);
        }
    }

    public void OnBoostButtonClick(Boost boost)
    {
        description.text = boost.description;
        _boostCache = boost;

        _rm.UpdateValues();
        if (boost.goldCost > 0) _rm.PreviewGoldChanging(boost.goldCost);
        if (boost.raspberylCost > 0) _rm.PreviewRaspberylChanging(boost.raspberylCost);
        if (boost.sapphireCost > 0) _rm.PreviewSapphireChanging(boost.sapphireCost);
    }

    public void OnAcceptButtonClick() 
    {
        if (_boostCache == null || !EnoughResources(_boostCache))
        {
            lockedSound.Play();
            return;
        }

        _rm.ChangeGoldValue(_boostCache.goldCost * -1);
        _rm.ChangeRaspberylValue(_boostCache.raspberylCost * -1);
        _rm.ChangeSapphireValue(_boostCache.sapphireCost * -1);

        _boostCache.button.enabled = false;
        _boostCache.outline.SetActive(true);

        switch (_boostCache.boostType)
        {
            case "shelterHP":
                ActivateShelterHpBoost();
                break;
            case "speed":
                ActivateSpeedBoost();
                break;
            case "drill":
                ActivateDrillBoost();
                break;
            case "gunPower":
                ActivateGunPowerBoost();
                break;
            case "gunRotateSpeed":
                ActivateGunRotateSpeedBoost();
                break;
            case "gunReloadingSpeed":
                ActivateGunReloadingSpeedBoost();
                break;
            case "jetpackLoadCapacity":
                ActivateJetpackLoadCapacityBoost();
                break;
        }

        _boostCache = null;
        
        CheckBoostsAvailability();
        CheckAnyBoostsAvailability();
    }

    private void ActivateShelterHpBoost()
    {
        if (_boostCache.id + 1 < shelterHpBoosts.Length) shelterHpBoosts[_boostCache.id + 1].button.interactable = true;

        Shelter.Instance.IncreaseMaxHp();
    }

    private void ActivateDrillBoost()
    {
        if (_boostCache.id + 1 < drillBoosts.Length) drillBoosts[_boostCache.id + 1].button.interactable = true;
        Player.Instance.UpgradeDrill();
    }

    private void ActivateSpeedBoost()
    {
        if (_boostCache.id + 1 < speedBoosts.Length) speedBoosts[_boostCache.id + 1].button.interactable = true;
        CurrentSpeedBoost += boostValue;
    }

    private void ActivateGunPowerBoost() 
    {
        if (_boostCache.id + 1 < bulletPowerBoosts.Length) bulletPowerBoosts[_boostCache.id + 1].button.interactable = true;
        Gun.Instance.IncreaseBulletPower();
    }

    private void ActivateGunRotateSpeedBoost()
    {
        if (_boostCache.id + 1 < gunRotateSpeedBoosts.Length)
            gunRotateSpeedBoosts[_boostCache.id + 1].button.interactable = true;
        Gun.Instance.IncreaseRotationSpeed();
    }
    
    private void ActivateGunReloadingSpeedBoost()
    {
        if (_boostCache.id + 1 < gunReloadingSpeedBoosts.Length)
            gunReloadingSpeedBoosts[_boostCache.id + 1].button.interactable = true;
        Gun.Instance.IncreaseReloadingSpeed();
    }

    private void ActivateJetpackLoadCapacityBoost()
    {
        if (_boostCache.id + 1 < jetpackLoadCapacityBoosts.Length)
            jetpackLoadCapacityBoosts[_boostCache.id + 1].button.interactable = true;
        Player.Instance.IncreaseJetpackLoadCapacity();
    }

    private bool EnoughResources(Boost boost) =>
        _rm.GoldCount >= boost.goldCost &&
        _rm.RaspberylCount >= boost.raspberylCost &&
        _rm.SapphireCount >= boost.sapphireCost;
}
