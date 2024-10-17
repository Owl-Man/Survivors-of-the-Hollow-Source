using System.Collections;
using DataBase;
using Localization;
using TMPro;
using UnityEngine;

public class PlanetTextEnter : MonoBehaviour
{
    [SerializeField] private TMP_Text planetDescription;

    private IEnumerator Start()
    {
        yield return null;
        
        if (LocalizationManager.Instance.GetLocalizedFont() != null)
            planetDescription.font = LocalizationManager.Instance.GetLocalizedFont();
        
        planetDescription.text = DB.Access.gameData.planetsDescription[DB.Access.gameData.ChosenPlanet];
        planetDescription.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        planetDescription.GetComponent<FadeAnimation>().EndFadeAnimCanvasGroup();
    }
}