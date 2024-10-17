using DataBase;
using Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class Upgrade : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private GameObject outline, lockObj;
        
        [TextArea(3,10)] public string description;
        public int upgradeID;
        public bool isAvailable;

        private void Start()
        {
            LocalizationManager.Instance.OnLanguageChanged += UpdateDescription;

            if (upgradeID == -1) isAvailable = true;
            else isAvailable = DB.Access.GetUpgradeReachedStatus(upgradeID);
            
            if (lockObj != null) lockObj.SetActive(!isAvailable);
            
            UpdateDescription();
        }
        
        public void UpdateDescription() => description = LocalizationManager.Instance.GetLocalizedValue("U" + (upgradeID + 1));

        public void ActivateUpgrade() => outline.SetActive(true);

        public void DeactivateUpgrade() => outline.SetActive(false);

        public Sprite GetIcon() => image.sprite;
    }
}