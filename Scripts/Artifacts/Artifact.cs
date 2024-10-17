using Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Artifacts
{
    public class Artifact : MonoBehaviour
    {
        public string type;
        public string description;

        public string Name { get; private set; }
        
        public Image icon;

        public int goldCost, raspberylCost, sapphireCost;

        public int maxCount = 1;
        public int currentCount;
        public bool activateState;
        
        [SerializeField] private GameObject outline;
        private Button _interactButton;

        private void Awake()
        {
            _interactButton = GetComponent<Button>();
            _interactButton.interactable = false;
            outline.SetActive(false);
        }

        private void Start()
        {
            LocalizationManager.Instance.OnLanguageChanged += UpdateLangValues;
            UpdateLangValues();
        }

        public void EnableInteraction() => _interactButton.interactable = true;
        
        public void Activate()
        {
            outline.SetActive(true);
            activateState = true;
        }

        public void UpdateLangValues()
        {
            description = LocalizationManager.Instance.GetLocalizedValue(type.ToLower());
            Name = LocalizationManager.Instance.GetLocalizedValue(type + " Title");
        }
    }
}