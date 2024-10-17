using UnityEngine;

namespace Localization
{
    public class BtnSwitchLang : MonoBehaviour
    {
        public void OnButtonClick() => LocalizationManager.Instance.CurrentLanguage = name;
    }
}
