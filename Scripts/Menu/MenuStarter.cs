using System.Collections;
using UnityEngine;

namespace Menu
{
    public class MenuStarter : MonoBehaviour
    {
        [SerializeField] private Animator recommendationPanel;

        private IEnumerator Start()
        {
            recommendationPanel.gameObject.SetActive(true);
            recommendationPanel.Play("Start");
            yield return new WaitForSeconds(1.6f);
            recommendationPanel.Play("End");
            yield return new WaitForSeconds(1.8f);
            recommendationPanel.gameObject.SetActive(false);
        }
    }
}