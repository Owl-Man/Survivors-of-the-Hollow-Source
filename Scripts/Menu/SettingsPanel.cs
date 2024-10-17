using DataBase;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private Outline[] allGraphicsButtons;
        [SerializeField] private Toggle musicToggle, sfxToggle, spfxToggle, bruhToggle;

        private void Start()
        {
            allGraphicsButtons[DB.Access.GraphicsQuality].enabled = true;
            musicToggle.isOn = DB.Access.IsMusicOn;
            sfxToggle.isOn = DB.Access.IsSFXOn;
            spfxToggle.isOn = DB.Access.IsSPFXOn;
            bruhToggle.isOn = DB.Access.IsBruhModeOn;
        }

        public void OnBruhClick() => DB.Access.IsBruhModeOn = bruhToggle.isOn;
        
        public void OnMusicClick()
        {
            DB.Access.IsMusicOn = musicToggle.isOn;
            MusicSystem.Instance.UpdateState();
        }

        public void OnSFXClick()
        {
            DB.Access.IsSFXOn = sfxToggle.isOn;
            SFX.Instance.UpdateState();
        }

        public void OnSPFXClick()
        {
            DB.Access.IsSPFXOn = spfxToggle.isOn;
            GraphicsQualitySystem.Instance.UpdateRainState();
        }

        public void OnHighGraphicsChange(Outline outline)
        {
            GraphicsButtonActivate(outline);
            DB.Access.GraphicsQuality = 2;
            GraphicsQualitySystem.Instance.UpdateGraphics();
        }

        public void OnMediumGraphicsChange(Outline outline)
        {
            GraphicsButtonActivate(outline);
            DB.Access.GraphicsQuality = 1;
            GraphicsQualitySystem.Instance.UpdateGraphics();
        }

        public void OnLowGraphicsChange(Outline outline)
        {
            GraphicsButtonActivate(outline);
            DB.Access.GraphicsQuality = 0;
            GraphicsQualitySystem.Instance.UpdateGraphics();
        }

        private void GraphicsButtonActivate(Outline outline)
        {
            for (int i = 0; i < allGraphicsButtons.Length; i++)
            {
                allGraphicsButtons[i].enabled = false;
            }

            outline.enabled = true;
        }
    }
}
