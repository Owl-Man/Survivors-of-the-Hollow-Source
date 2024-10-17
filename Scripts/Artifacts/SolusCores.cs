using System.Collections;
using NTC.Global.Pool;
using UnityEngine;
using UnityEngine.UI;

namespace Artifacts
{
    public class SolusCores : MonoBehaviour
    {
        [SerializeField] private GameObject coresEffect;
        [SerializeField] private Image solusCoresBtn;
        
        private bool _isCoreActive;
        private bool _isCoreReady = true;

        public static SolusCores Instance;

        private void Start() => Instance = this;

        public void ActivateCoreArtifact()
        {
            _isCoreActive = true;
            CameraFocus.OnShelterEnter.AddListener(DisableArtBtn);
            CameraFocus.OnShelterExit.AddListener(EnableArtBtn);
        }

        private void EnableArtBtn() => solusCoresBtn.gameObject.SetActive(true);
        
        private void DisableArtBtn() => solusCoresBtn.gameObject.SetActive(false);


        public void OnCoreClick()
        {
            if (!_isCoreReady || !_isCoreActive) return;
            
            transform.SetPositionAndRotation(new Vector2(0, -4), Quaternion.identity);
            _isCoreReady = false;
            
            StartCoroutine(Effect());
            StartCoroutine(ChargeCore());
        }

        private IEnumerator ChargeCore()
        {
            float time = 0;
            solusCoresBtn.fillAmount = 0;

            while (time < 55)
            {
                yield return new WaitForSeconds(0.1f);
                time += 0.1f;
                solusCoresBtn.fillAmount = time * 100 / 55 / 100;
            }

            _isCoreReady = true;
        }

        private IEnumerator Effect()
        {
            GameObject effect = NightPool.Spawn(coresEffect, new Vector2(transform.position.x, transform.position.y - 1f));
            yield return new WaitForSeconds(1.2f);
            effect.GetComponent<ParticleSystem>().Stop();
            yield return new WaitForSeconds(1);
            NightPool.Despawn(effect);
        }
    }
}