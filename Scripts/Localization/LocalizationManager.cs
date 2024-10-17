using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        private Dictionary<string, string> _localizedText;
        public static bool IsReady;
        private string _currentLanguage;

        public delegate void ChangeLangText();
        public event ChangeLangText OnLanguageChanged;

        public TMP_FontAsset defaultFont, jpFont, krFont, ruFont;

        public static LocalizationManager Instance;

        public string CurrentLanguage
        {
            get { return _currentLanguage; }
            set
            {
                PlayerPrefs.SetString("Language", value);
                _currentLanguage = PlayerPrefs.GetString("Language");
                LoadLocalizedText(_currentLanguage);
            }
        }

        private void Awake()
        {
            Instance = this;

            if (!PlayerPrefs.HasKey("Language"))
            {
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Turkish:
                        PlayerPrefs.SetString("Language", "tr_TR");
                        break;
                    
                    case SystemLanguage.German:
                        PlayerPrefs.SetString("Language", "de_DE");
                        break;
                    
                    case SystemLanguage.Japanese:
                        PlayerPrefs.SetString("Language", "jp_JP");
                        break;
                    
                    case SystemLanguage.Korean:
                        PlayerPrefs.SetString("Language", "kr_KR");
                        break;
                    
                    case SystemLanguage.Finnish:
                    case SystemLanguage.Swedish:
                        PlayerPrefs.SetString("Language", "sw_SW");
                        break;
                    
                    case SystemLanguage.Belarusian:
                    case SystemLanguage.Ukrainian:
                    case SystemLanguage.Russian:
                        PlayerPrefs.SetString("Language", "ru_RU");
                        break;
                    
                    case SystemLanguage.Spanish:
                        PlayerPrefs.SetString("Language", "es_ES");
                        break;
                    
                    default:
                        PlayerPrefs.SetString("Language", "en_US");
                        break;
                }
            }

            _currentLanguage = PlayerPrefs.GetString("Language");
            LoadLocalizedText(_currentLanguage);
        }

        public TMP_FontAsset GetLocalizedFont()
        {
            if (_currentLanguage == "jp_JP") return jpFont;
            if (_currentLanguage == "kr_KR") return krFont;
            if (_currentLanguage == "ru_RU") return ruFont;

            return null;
        } 

        public void LoadLocalizedText(string langName)
        {
            string path = Application.streamingAssetsPath + "/Languages/" + langName + ".json";
            string dataAsJson;

            if (Application.platform == RuntimePlatform.Android)
            {
                WWW reader = new WWW(path);

                while (!reader.isDone) { }

                dataAsJson = reader.text;

            }
            else
            {
                dataAsJson = File.ReadAllText(path);
            }

            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            _localizedText = new Dictionary<string, string>();

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                _localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            PlayerPrefs.SetString("Language", langName);

            IsReady = true;

            OnLanguageChanged?.Invoke();

        }

        public string GetLocalizedValue(string key) 
        {
            if (_localizedText.ContainsKey(key)) return _localizedText[key];
            else throw new Exception("Localization text with key \"" + key + "\" not found");
        }
    }
}
