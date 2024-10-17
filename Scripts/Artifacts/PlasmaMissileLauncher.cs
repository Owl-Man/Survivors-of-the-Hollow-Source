using System.Collections;
using NTC.Global.Pool;
using UnityEngine;
using UnityEngine.UI;

namespace Artifacts
{
    public class PlasmaMissileLauncher : MonoBehaviour
    {
        [SerializeField] private GameObject missile;
        [SerializeField] private Image launchBtn;

        private bool _isMissilesReady = true;

        public void OnLaunchClick()
        {
            if (!_isMissilesReady) return;
            
            if (ResourcesManager.Instance.RaspberylCount < 1) return;
            
            ResourcesManager.Instance.ChangeRaspberylValue(-1);

            _isMissilesReady = false;
            
            StartCoroutine(StartLaunching());
        }

        private IEnumerator StartLaunching()
        {
            StartCoroutine(ChargeLauncher());
            
            for (int i = 0; i < 12; i++)
            {
                NightPool.Spawn(missile, transform.position);
                yield return new WaitForSeconds(0.2f);
            }
        }
        
        private IEnumerator ChargeLauncher()
        {
            float time = 0;
            launchBtn.fillAmount = 0;

            while (time < 10)
            {
                yield return new WaitForSeconds(0.1f);
                time += 0.1f;
                launchBtn.fillAmount = time * 100 / 10 / 100;
            }

            _isMissilesReady = true;
        }
    }
}