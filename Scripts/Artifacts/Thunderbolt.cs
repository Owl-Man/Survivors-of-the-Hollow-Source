using System.Collections;
using EnemySystem;
using NTC.Global.Pool;
using UnityEngine;

namespace Artifacts
{
    public class Thunderbolt : MonoBehaviour
    {
        [SerializeField] private GameObject mainBlastLight;
        
        private LineRenderer _lineRenderer;

        private bool _isThunderReady = true;
        private float _cooldown = 5;

        private bool _isBlast;

        private void Start() => _lineRenderer = GetComponent<LineRenderer>();

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (_isThunderReady)
                {
                    _isThunderReady = false;
                    StartCoroutine(Blast(other.transform));
                }
            }
        }

        private IEnumerator Blast(Transform target)
        {
            GameObject onTargetLight = NightPool.Spawn(mainBlastLight, target);
            onTargetLight.transform.localScale = new Vector2(0.385f, 0.21f);
            mainBlastLight.SetActive(true);
            _isBlast = true;
            
            StartCoroutine(Cooldown());
            StartCoroutine(BlastTime());

            while (true)
            {
                onTargetLight.transform.position = target.position;
                
                _lineRenderer.positionCount = 2;
                
                if (!_isBlast)
                {
                    _lineRenderer.positionCount = 0;
                    break;
                }

                Vector3[] line = new Vector3[2];
                line[0] = transform.position;
                line[1] = target.transform.position;

                _lineRenderer.SetPositions(line);
                
                yield return null;
            }
            
            target.GetComponent<Enemy>().TakeDamage(1, target.position);
            mainBlastLight.SetActive(false);
            NightPool.Despawn(onTargetLight);
        }

        private IEnumerator BlastTime()
        {
            yield return new WaitForSeconds(1.5f);
            _isBlast = false;
        }

        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(_cooldown);
            _isThunderReady = true;
        }
    }
}