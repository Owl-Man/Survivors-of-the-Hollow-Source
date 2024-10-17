using UnityEngine;

namespace GunSystem
{
    public class Bullet : BulletBase
    {
        [SerializeField] private TrailRenderer trail;

        protected override void OnDisable()
        {
            base.OnDisable();
            trail.Clear();
        }
    
        protected override void DestroyEffect() => Effects.Instance.RedBlow(transform.position, isWithAdditionalExp, true);
    }
}
