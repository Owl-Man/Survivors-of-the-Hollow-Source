using System.Collections;
using NTC.Global.Cache;
using NTC.Global.Pool;
using UnityEngine;

namespace GunSystem
{
    public abstract class BulletBase : NightCache, INightRun
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] protected Transform towardPoint;

        [SerializeField] protected bool isWithAdditionalExp;

        protected string TargetTag = "Block";
        protected float MaxDistanceDelta = 2f;

        private void OnEnable() => StartCoroutine(DisableWhenInInfinityFly());
    
        protected virtual void OnDisable() => StopCoroutine(DisableWhenInInfinityFly());

        private IEnumerator DisableWhenInInfinityFly()
        {
            yield return new WaitForSeconds(2);
            NightPool.Despawn(this);
        }

        public void Run() => rb.MovePosition(Vector2.MoveTowards(transform.position, towardPoint.transform.position, MaxDistanceDelta));

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(TargetTag))
            {
                DestroyEffect();
                NightPool.Despawn(this);
            }
        }
    
        protected virtual void DestroyEffect() { }
    }
}