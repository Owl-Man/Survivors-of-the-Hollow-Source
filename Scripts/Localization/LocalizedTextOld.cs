using UnityEngine;
using UnityEngine.UI;

namespace Localization
{
    public class LocalizedTextOld : MonoBehaviour
    {
        [SerializeField] private string key;

        private LocalizationManager _localizationManager;
        private Text _text;

        private void Start()
        {
            _localizationManager = LocalizationManager.Instance;

            _text = GetComponent<Text>();

            _localizationManager.OnLanguageChanged += UpdateText;

            UpdateText();
        }

        private void OnDestroy() => _localizationManager.OnLanguageChanged -= UpdateText;

        virtual protected void UpdateText()
        {
            if (gameObject == null) return;

            _text.text = _localizationManager.GetLocalizedValue(key);
        }
    }
}