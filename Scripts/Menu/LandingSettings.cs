using System.Collections;
using DataBase;
using Google_ADS_System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class LandingSettings : MonoBehaviour
    {
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private TMP_Text description;
        [SerializeField] private GameObject randPlanetShow;
        [SerializeField] private Image selectedUpgrade;

        [SerializeField] private Button landPageButton, inventoryPageButton;
        private Outline _landBtnOutline, _inventoryBtnOutline;
        [SerializeField] private Image chosenPlanetImage;

        [SerializeField] private GameObject landPage, inventoryPage;
        [SerializeField] private GameObject invAvailableNotify;

        [SerializeField] private Planet random;
        [SerializeField] private Upgrade withoutUpgrade;

        private Planet _chosenPlanet;
        private Upgrade _chosenUpgrade;

        private void Start()
        {
            _landBtnOutline = landPageButton.GetComponent<Outline>();
            _inventoryBtnOutline = inventoryPageButton.GetComponent<Outline>();

            _landBtnOutline.enabled = true;
            _inventoryBtnOutline.enabled = false;

            _chosenPlanet = random;

            withoutUpgrade.ActivateUpgrade();
            selectedUpgrade.enabled = false;
            _chosenUpgrade = withoutUpgrade;
            
            CheckAvailableUpgradeForChoose();
        }

        public void CheckAvailableUpgradeForChoose()
        {
            if (_chosenUpgrade != withoutUpgrade)
            {
                invAvailableNotify.SetActive(false);
                return;
            }

            for (int i = 0; i < 6; i++)
            {
                if (DB.Access.GetUpgradeReachedStatus(i))
                {
                    invAvailableNotify.SetActive(true);
                    return;
                }
            }
            
            invAvailableNotify.SetActive(false);
        }

        public void StartLanding()
        {
            if (Random.Range(0, 3) == 0) InterAd.Instance.ShowAd();
            
            DB.Access.gameData.ChosenPlanet = _chosenPlanet.planetID;

            if (_chosenUpgrade != null && _chosenUpgrade.isAvailable)
                DB.Access.gameData.chosenUpgrade = _chosenUpgrade.upgradeID;
            else DB.Access.gameData.chosenUpgrade = withoutUpgrade.upgradeID;

            StartCoroutine(StartLoadingGameScene());
        }

        private IEnumerator StartLoadingGameScene()
        {
            loadingPanel.SetActive(true);
            yield return new WaitForSeconds(2.4f);
            SceneManager.LoadScene("Game");
        }

        public void OnLandPageButtonClick()
        {
            landPage.SetActive(true);

            StartCoroutine(OpenPage(true));

            _landBtnOutline.enabled = true;
            _inventoryBtnOutline.enabled = false;
        }

        public void OnInventoryPageButtonClick()
        {
            inventoryPage.SetActive(true);

            StartCoroutine(OpenPage(false));

            _landBtnOutline.enabled = false;
            _inventoryBtnOutline.enabled = true;
        }
        
        private IEnumerator OpenPage(bool isLand)
        {
            landPageButton.enabled = false;
            inventoryPageButton.enabled = false;

            yield return new WaitForSeconds(0.7f);

            landPageButton.enabled = true;
            inventoryPageButton.enabled = true;

            if (isLand)
            {
                landPageButton.interactable = false;
                inventoryPageButton.interactable = true;
            }
            else
            {
                landPageButton.interactable = true;
                inventoryPageButton.interactable = false;
            }
        }

        public void PlanetChoose(Planet planet)
        {
            if (_chosenPlanet == null) ActivatePlanet(planet);
            else if (_chosenPlanet.name != planet.name)
            {
                _chosenPlanet.DeactivatePlanet();
                ActivatePlanet(planet);
            }
        }

        public void UpgradeChoose(Upgrade upgrade)
        {
            if (_chosenUpgrade == null) ActivateUpgrade(upgrade);
            else if (_chosenUpgrade.name != upgrade.name)
            {
                _chosenUpgrade.DeactivateUpgrade();
                ActivateUpgrade(upgrade);
            }
            else
            {
                upgrade.DeactivateUpgrade();
                description.text = upgrade.description;
                _chosenUpgrade = null;
            }
        }

        private void ActivatePlanet(Planet planet)
        {
            planet.ActivatePlanet();
            
            if (planet.icon != null)
            {
                chosenPlanetImage.sprite = planet.icon;
                randPlanetShow.SetActive(false);
                chosenPlanetImage.color = new Color(255, 255, 255, 255);
            }
            else
            {
                randPlanetShow.SetActive(true);
                chosenPlanetImage.color = new Color(0, 0, 0, 255);
            }

            _chosenPlanet = planet;
        }

        private void ActivateUpgrade(Upgrade upgrade)
        {
            upgrade.ActivateUpgrade();
            description.text = upgrade.description;
            _chosenUpgrade = upgrade;
            
            if (upgrade.isAvailable)
            {
                CheckAvailableUpgradeForChoose();
                
                if (upgrade.upgradeID == -1) selectedUpgrade.enabled = false;
                else
                {
                    selectedUpgrade.enabled = true;
                    selectedUpgrade.sprite = upgrade.GetIcon();
                }
            }
        }
    }
}
