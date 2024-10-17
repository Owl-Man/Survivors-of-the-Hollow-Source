using System.Collections;
using Localization;
using TMPro;
using UnityEngine;

public class LoadText : MonoBehaviour
{
    private TMP_Text _textCmp;

    private IEnumerator Start()
    {
        _textCmp = GetComponent<TMP_Text>();

        if (LocalizationManager.Instance.GetLocalizedFont() != null)
            _textCmp.font = LocalizationManager.Instance.GetLocalizedFont();
        
        _textCmp.text = LocalizationManager.Instance.GetLocalizedValue("Analyzing planet");
        yield return new WaitForSeconds(1.5f);
        _textCmp.text = LocalizationManager.Instance.GetLocalizedValue("Map generation");
    }
}
