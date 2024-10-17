using System.Collections;
using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Artifacts
{
    public class NewArtifact : MonoBehaviour
    {
        [SerializeField] private GameObject showArtifactChoosePanel, showNewArtifactPanel, showArtifactConversionPanel;

        [SerializeField] private Button[] artChooseBtns;
        [SerializeField] private TMP_Text[] artChoosePanelNames;
        [SerializeField] private TMP_Text[] artChoosePanelDescriptions;
        [SerializeField] private Image[] artChoosePanelIcons;

        [SerializeField] private TMP_Text artName;
        [SerializeField] private TMP_Text artDescription;
        [SerializeField] private Image artIcon;

        [SerializeField] private Sprite emptyArtIcon;

        private TMP_FontAsset _defaultFont;

        private FadeAnimation _artConversionAnim;
        private bool _isArtifactShowIsBusy;

        private void Start()
        {
            _artConversionAnim = showArtifactConversionPanel.GetComponent<FadeAnimation>();
            _defaultFont = artName.font;
            LocalizationManager.Instance.OnLanguageChanged += UpdateFont;
            UpdateFont();
        }

        public void ShowArtifactChoosePanel(Artifact a1, Artifact a2, Artifact a3)
        {
            Time.timeScale = 0f;
            
            a1.UpdateLangValues();
            a2.UpdateLangValues();
            a3.UpdateLangValues();
            
            showArtifactChoosePanel.SetActive(true);

            artChooseBtns[0].interactable = true;
            artChooseBtns[1].interactable = true;
            artChooseBtns[2].interactable = true;
            
            artChoosePanelNames[0].text = a1.Name;
            artChoosePanelDescriptions[0].text = a1.description;
            artChoosePanelIcons[0].sprite = a1.icon.sprite;
            artChoosePanelIcons[0].color = a1.icon.color;
            
            artChoosePanelNames[1].text = a2.Name;
            artChoosePanelDescriptions[1].text = a2.description;
            artChoosePanelIcons[1].sprite = a2.icon.sprite;
            artChoosePanelIcons[1].color = a2.icon.color;
            
            artChoosePanelNames[2].text = a3.Name;
            artChoosePanelDescriptions[2].text = a3.description;
            artChoosePanelIcons[2].sprite = a3.icon.sprite;
            artChoosePanelIcons[2].color = a3.icon.color;
        }
        
        public void ShowArtifactChoosePanel(Artifact a1, Artifact a2)
        {
            Time.timeScale = 0f;
            
            a1.UpdateLangValues();
            a2.UpdateLangValues();
            
            showArtifactChoosePanel.SetActive(true);
            
            artChooseBtns[0].interactable = true;
            artChooseBtns[1].interactable = true;
            artChooseBtns[2].interactable = false;
            
            artChoosePanelNames[0].text = a1.Name;
            artChoosePanelDescriptions[0].text = a1.description;
            artChoosePanelIcons[0].sprite = a1.icon.sprite;
            artChoosePanelIcons[0].color = a1.icon.color;
            
            artChoosePanelNames[1].text = a2.Name;
            artChoosePanelDescriptions[1].text = a2.description;
            artChoosePanelIcons[1].sprite = a2.icon.sprite;
            artChoosePanelIcons[1].color = a2.icon.color;
            
            artChoosePanelNames[2].text = LocalizationManager.Instance.GetLocalizedValue("U0");;
            artChoosePanelDescriptions[2].text = "";
            artChoosePanelIcons[2].sprite = emptyArtIcon;
            artChoosePanelIcons[2].color = new Color(255, 255, 255, 1);
        }
        
        public void ShowArtifactChoosePanel(Artifact a1)
        {
            Time.timeScale = 0f;
            
            a1.UpdateLangValues();
            
            showArtifactChoosePanel.SetActive(true);
            
            artChooseBtns[0].interactable = true;
            artChooseBtns[1].interactable = false;
            artChooseBtns[2].interactable = false;
            
            artChoosePanelNames[0].text = a1.Name;
            artChoosePanelDescriptions[0].text = a1.description;
            artChoosePanelIcons[0].sprite = a1.icon.sprite;
            artChoosePanelIcons[0].color = a1.icon.color;
            
            artChoosePanelNames[1].text = LocalizationManager.Instance.GetLocalizedValue("U0");
            artChoosePanelDescriptions[1].text = "";
            artChoosePanelIcons[1].sprite = emptyArtIcon;
            artChoosePanelIcons[1].color = new Color(255, 255, 255, 1);
            
            artChoosePanelNames[2].text = LocalizationManager.Instance.GetLocalizedValue("U0");;
            artChoosePanelDescriptions[2].text = "";
            artChoosePanelIcons[2].sprite = emptyArtIcon;
            artChoosePanelIcons[2].color = new Color(255, 255, 255, 1);
        }

        public void OnArtifactChooseClick(int chooseID) => ArtifactsManager.Instance.ActivateArtifactInteraction(chooseID);

        public void ShowNewArtifact(Artifact artifact)
        {
            showNewArtifactPanel.SetActive(true);

            artifact.UpdateLangValues();
            
            artName.text = artifact.Name;
            
            artDescription.text = artifact.description;
            
            artIcon.sprite = artifact.icon.sprite;
            artIcon.color = artifact.icon.color;
        }
        
        public void ShowArtifactConversion()
        {
            ResourcesManager.Instance.ChangeGoldValue(5);
            ResourcesManager.Instance.ChangeRaspberylValue(3);
            ResourcesManager.Instance.ChangeSapphireValue(1);
            
            StartCoroutine(ArtifactConversion());
        }

        private IEnumerator ArtifactConversion()
        {
            showArtifactConversionPanel.SetActive(true);
            yield return new WaitForSeconds(7);
            _artConversionAnim.EndFadeAnimCanvasGroup();
        }

        public void OnOKClick()
        {
            Time.timeScale = 1f;
            showNewArtifactPanel.SetActive(false);
            
            ArtifactsManager.Instance.CheckForArtifactInQueue();
        }
        
        public void UpdateFont()
        {
            if (LocalizationManager.Instance.GetLocalizedFont() != null)
            {
                artName.font = LocalizationManager.Instance.GetLocalizedFont();
                artDescription.font = LocalizationManager.Instance.GetLocalizedFont();

                for (int i = 0; i < artChoosePanelNames.Length; i++)
                {
                    artChoosePanelNames[i].font = LocalizationManager.Instance.GetLocalizedFont();
                }
                
                for (int i = 0; i < artChoosePanelDescriptions.Length; i++)
                {
                    artChoosePanelDescriptions[i].font = LocalizationManager.Instance.GetLocalizedFont();
                }
            }
            else
            {
                artName.font = _defaultFont;
                artDescription.font = _defaultFont;
                
                
                for (int i = 0; i < artChoosePanelNames.Length; i++)
                {
                    artChoosePanelNames[i].font = _defaultFont;
                }
                
                for (int i = 0; i < artChoosePanelDescriptions.Length; i++)
                {
                    artChoosePanelDescriptions[i].font = _defaultFont;
                }
            }
        }
    }
}