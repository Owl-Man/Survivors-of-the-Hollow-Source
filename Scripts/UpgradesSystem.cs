using System;
using System.Collections;
using DataBase;
using Google_Play_Games_System;
using Localization;
using TMPro;
using UnityEngine;

public class UpgradesSystem : MonoBehaviour
{
    [SerializeField] private GameObject achievementPanel;
    [SerializeField] private TMP_Text descriptionTMP;
    private DB _dataBase;
    
    public static UpgradesSystem Instance;

    private TMP_FontAsset _defaultFont;

    private string _descriptionTemplate(int count, string upgrade)
        => String.Format(LocalizationManager.Instance.GetLocalizedValue("AchievmentDescriptionTemplate"), count, upgrade);
    
    private void Awake() => Instance = this;

    private void Start()
    {
        _defaultFont = descriptionTMP.font;
        LocalizationManager.Instance.OnLanguageChanged += UpdateFont;
        UpdateFont();
        _dataBase = DB.Access;
    }

    public void CheckForNewAchievement(ushort days)
    {
        _dataBase.MaxSurviveDays = Math.Max(days, _dataBase.MaxSurviveDays);

        if (!_dataBase.GetUpgradeReachedStatus(0) && _dataBase.MaxSurviveDays >= _dataBase.gameData.DaysRequirementForUpgrade[0])
        {
            StartCoroutine(ShowAchievement(_descriptionTemplate(_dataBase.gameData.DaysRequirementForUpgrade[0], "extra fuel")));
            _dataBase.SetUpgradeReachedStatus(0, true);
            AchievementsSystem.Instance.Unlock(GPGIDs.extra_fuel_achievement);
        }
        else if (!_dataBase.GetUpgradeReachedStatus(1) && _dataBase.MaxSurviveDays >= _dataBase.gameData.DaysRequirementForUpgrade[1])
        {
            StartCoroutine(ShowAchievement(_descriptionTemplate(_dataBase.gameData.DaysRequirementForUpgrade[1], "superior material")));
            _dataBase.SetUpgradeReachedStatus(1, true);
            AchievementsSystem.Instance.Unlock(GPGIDs.superior_material_achievement);
        }
        else if (!_dataBase.GetUpgradeReachedStatus(2) && _dataBase.MaxSurviveDays >= _dataBase.gameData.DaysRequirementForUpgrade[2])
        {
            StartCoroutine(ShowAchievement(_descriptionTemplate(_dataBase.gameData.DaysRequirementForUpgrade[2], "efficient processing")));
            _dataBase.SetUpgradeReachedStatus(2, true);
            AchievementsSystem.Instance.Unlock(GPGIDs.efficient_processing_achievement);
        }
        else if (!_dataBase.GetUpgradeReachedStatus(3) && _dataBase.MaxSurviveDays >= _dataBase.gameData.DaysRequirementForUpgrade[3])
        {
            StartCoroutine(ShowAchievement(_descriptionTemplate(_dataBase.gameData.DaysRequirementForUpgrade[3], "cumulative missile")));
            _dataBase.SetUpgradeReachedStatus(3, true);
            AchievementsSystem.Instance.Unlock(GPGIDs.cumulative_missile_achievement);
        }
        else if (!_dataBase.GetUpgradeReachedStatus(4)
                 && _dataBase.MaxSurviveDays >= _dataBase.gameData.DaysRequirementForUpgrade[4]
                 && _dataBase.gameDifficult == 2)
        {
            StartCoroutine(ShowAchievement(_descriptionTemplate(_dataBase.gameData.DaysRequirementForUpgrade[4], "drill amplifier")));
            _dataBase.SetUpgradeReachedStatus(4, true);
            AchievementsSystem.Instance.Unlock(GPGIDs.drill_amplifier_achievement);
        }
        else if (!_dataBase.GetUpgradeReachedStatus(5)
                 && _dataBase.MaxSurviveDays >= _dataBase.gameData.DaysRequirementForUpgrade[5]
                 && _dataBase.gameDifficult == 3)
        {
            StartCoroutine(ShowAchievement(_descriptionTemplate(_dataBase.gameData.DaysRequirementForUpgrade[5], "artifact package")));
            _dataBase.SetUpgradeReachedStatus(5, true);
            AchievementsSystem.Instance.Unlock(GPGIDs.artifact_package_achievement);
        }
    }

    private IEnumerator ShowAchievement(string description)
    {
        achievementPanel.SetActive(true);
        descriptionTMP.text = description;
        yield return new WaitForSeconds(7f);
        achievementPanel.GetComponent<FadeAnimation>().EndFadeAnimCanvasGroup();
    }

    private void UpdateFont()
    {
        if (LocalizationManager.Instance.GetLocalizedFont() != null)
        {
            descriptionTMP.font = LocalizationManager.Instance.GetLocalizedFont();
        }
        else descriptionTMP.font = _defaultFont;
    }
}