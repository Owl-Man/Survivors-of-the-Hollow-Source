using System.Collections;
using DataBase;
using Localization;
using TMPro;
using UnityEngine;

namespace Menu
{
    public class MaxSurviveText : MonoBehaviour
    {
        private TMP_Text _text;

        private void OnEnable()
        {
            if (_text == null) _text = GetComponent<TMP_Text>();
            StartCoroutine(UpdatingText());
        }

        private void OnDisable() => StopAllCoroutines();

        private IEnumerator UpdatingText()
        {
            _text.text = LocalizationManager.Instance.GetLocalizedValue("MAX SURVIVED WAVES");
            yield return new WaitForSeconds(0.2f);
            _text.text += DB.Access.MaxSurviveDays;
        }
    }
}