using UnityEngine;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private GameObject landingSettingsPanel;
        
        private void Start() => Time.timeScale = 1;

        public void OnPlayButtonClick() => landingSettingsPanel.SetActive(true);

        public void OnExitButtonClick() => Application.Quit();
    }
}
