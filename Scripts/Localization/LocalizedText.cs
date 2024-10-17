using TMPro;
using UnityEngine;

namespace Localization
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string key;

        private LocalizationManager _localizationManager;
        private TMP_Text _text;

        private TMP_FontAsset _defaultFont;

        private void Start()
        {
            _localizationManager = LocalizationManager.Instance;

            _text = GetComponent<TMP_Text>();

            _defaultFont = _text.font;

            _localizationManager.OnLanguageChanged += UpdateText;

            UpdateText();
        }

        private void OnDestroy() => _localizationManager.OnLanguageChanged -= UpdateText;

        protected virtual void UpdateText()
        {
            if (gameObject == null) return;

            _text.text = _localizationManager.GetLocalizedValue(key);

            if (_localizationManager.GetLocalizedFont() != null) _text.font = _localizationManager.GetLocalizedFont();
            else _text.font = _defaultFont;
        }
    }
}