using System.Collections;
using NTC.Global.Cache;
using NTC.Global.Pool;
using UnityEngine;

namespace Artifacts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class HomingMissile : NightCache, INightFixedRun
    {
        [SerializeField] private TrailRenderer trailRenderer;
        
        private Rigidbody2D _rb;
        private Transform _target;

        public float speed = 5f;
        public float rotateSpeed = 200f;

        private bool _isTargetChosen;

        private void Start() => _rb = GetComponent<Rigidbody2D>();

        private void OnEnable()
        {
            _isTargetChosen = false;
            StartCoroutine(InfinityFly());
        }

        private void OnDisable() => trailRenderer.Clear();

        private IEnumerator InfinityFly()
        {
            yield return new WaitForSeconds(3);
            NightPool.Despawn(gameObject);
        }

        public void FixedRun() 
        {
            if (!_isTargetChosen) _rb.velocity = transform.up * speed;
            else
            {
                Vector2 direction = (Vector2)_target.position - _rb.position;

                direction.Normalize();

                float rotateAmount = Vector3.Cross(direction, transform.up).z;

                _rb.angularVelocity = -rotateAmount * rotateSpeed;

                _rb.velocity = transform.up * speed;   
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && !_isTargetChosen)
            {
                _target = other.transform;
                _isTargetChosen = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Block"))
            {
                Effects.Instance.RedBlow(transform.position, true, true);
                NightPool.Despawn(gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_isTargetChosen && other.transform == _target.transform)
            {
                _isTargetChosen = false;
            }
        }
    }
}