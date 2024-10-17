using Google_Play_Games_System;
using Localization;
using UnityEngine;

namespace DataBase
{
    public class DB : MonoBehaviour
    {
        private int _graphicsQuality;

        public int GraphicsQuality
        {
            get => _graphicsQuality;
            set
            {
                if (_graphicsQuality != value) PlayerPrefs.SetInt("GraphicsQuality", value);
                _graphicsQuality = value;
            }
        }

        private int _maxSurviveDays;

        public int MaxSurviveDays
        {
            get => _maxSurviveDays;
            set
            {
                if (_maxSurviveDays != value) PlayerPrefs.SetInt("MaxSurviveDays", value);
                _maxSurviveDays = value;
            }
        }

        public bool IsMusicOn
        {
            get => _isMusicOn;
            set
            {
                _isMusicOn = value;

                PlayerPrefs.SetInt("MusicState", _isMusicOn ? 1 : 0);
            }
        }

        public bool IsSFXOn
        {
            get => _isSFXOn;
            set
            {
                _isSFXOn = value;

                PlayerPrefs.SetInt("SFXState", _isSFXOn ? 1 : 0);
            }
        }
        
        public bool IsSPFXOn
        {
            get => _isSPFXOn;
            set
            {
                _isSPFXOn = value;

                PlayerPrefs.SetInt("SPFXState", _isSPFXOn ? 1 : 0);
            }
        }

        public bool IsBruhModeOn
        {
            get => _isBruhModeOn;
            set
            {
                _isBruhModeOn = value;

                PlayerPrefs.SetInt("BruhMode", _isBruhModeOn ? 1 : 0);
            }
        }

        private bool _isMusicOn, _isSFXOn, _isSPFXOn, _isBruhModeOn;

        private bool[] _isUpgradeReached = {false, false, false, false, false, false};

        public void SetUpgradeReachedStatus(int index, bool value)
        {
            _isUpgradeReached[index] = value;
            PlayerPrefs.SetInt("IsUpgradeReached" + index, _isUpgradeReached[index] ? 1 : 0);
        }

        public bool GetUpgradeReachedStatus(int index) => _isUpgradeReached[index];

        [HideInInspector] public int gameDifficult;
        
        public GameData gameData;
        public static DB Access;

        public const int HighGraphicsQuality = 2;
        public const int MediumGraphicsQuality = 1;
        public const int LowGraphicsQuality = 0;

        private void Awake()
        {
            if (Access)
            {
                Destroy(gameObject);
                return;
            }

            Access = this;

            gameDifficult = 1;

            _maxSurviveDays = PlayerPrefs.GetInt("MaxSurviveDays", 0);
            _graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", 2);
            
            _isMusicOn = PlayerPrefs.GetInt("MusicState", 1) == 1;
            _isSFXOn = PlayerPrefs.GetInt("SFXState", 1) == 1;
            _isSPFXOn = PlayerPrefs.GetInt("SPFXState", 1) == 1;
            
            _isBruhModeOn = PlayerPrefs.GetInt("BruhMode", 0) == 1;

            for (int i = 0; i < _isUpgradeReached.Length; i++)
            {
                SetUpgradeReachedStatus(i, PlayerPrefs.GetInt("IsUpgradeReached" + i, 0) == 1);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            LocalizationManager.Instance.OnLanguageChanged += PlanetsDescriptionUpdate;
            PlanetsDescriptionUpdate();
            
            if (PlayerPrefs.GetInt("IsAchievementsSynced", 0) == 0)
            {
                for (int i = 0; i < _isUpgradeReached.Length; i++)
                {
                    if (_isUpgradeReached[i])
                    {
                        if (i == 0) AchievementsSystem.Instance.Unlock(GPGIDs.extra_fuel_achievement);
                        else if (i == 1) AchievementsSystem.Instance.Unlock(GPGIDs.superior_material_achievement);
                        else if (i == 2) AchievementsSystem.Instance.Unlock(GPGIDs.efficient_processing_achievement);
                        else if (i == 3) AchievementsSystem.Instance.Unlock(GPGIDs.cumulative_missile_achievement);
                        else if (i == 4) AchievementsSystem.Instance.Unlock(GPGIDs.drill_amplifier_achievement);
                        else if (i == 5) AchievementsSystem.Instance.Unlock(GPGIDs.artifact_package_achievement);
                    }
                }
                
                PlayerPrefs.SetInt("IsAchievementsSynced", 1);
            }
        }

        private void PlanetsDescriptionUpdate()
        {
            gameData.planetsDescription[0] = LocalizationManager.Instance.GetLocalizedValue("Sayen");
            gameData.planetsDescription[1] = LocalizationManager.Instance.GetLocalizedValue("Eokcon");
            gameData.planetsDescription[2] = LocalizationManager.Instance.GetLocalizedValue("Getkra");
            gameData.planetsDescription[3] = LocalizationManager.Instance.GetLocalizedValue("Koita");
            gameData.planetsDescription[4] = LocalizationManager.Instance.GetLocalizedValue("Argen");
            gameData.planetsDescription[5] = LocalizationManager.Instance.GetLocalizedValue("Fark");
        }
    }
}