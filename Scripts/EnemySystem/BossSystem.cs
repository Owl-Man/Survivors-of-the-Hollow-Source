using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EnemySystem
{
    public class BossSystem : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject bossHp;
        [SerializeField] private Image bossHpBar;
        [SerializeField] private TMP_Text bossNameField;

        [SerializeField] private FadeAnimation alert;

        [Header("Values")]
        [SerializeField] private string[] bossNames;

        public void ChangeHpValue(float value) //in range (0f, 1f)
        {
            bossHpBar.fillAmount = value;
        }
        
        public void Begin(int bossID)
        {
            bossHp.SetActive(true);
            bossNameField.text = bossNames[bossID];

            StartCoroutine(Alert());
        }

        private IEnumerator Alert()
        {
            alert.gameObject.SetActive(true);
            yield return new WaitForSeconds(5);
            alert.EndFadeAnimCanvasGroup();
        }

        public void End()
        {
            bossHpBar.fillAmount = 1;
            bossHp.SetActive(false);
        }
    }
}