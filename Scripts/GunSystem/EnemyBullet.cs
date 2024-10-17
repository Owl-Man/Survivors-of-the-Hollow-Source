using NTC.Global.Cache;
using UnityEngine;

namespace GunSystem
{
    public class EnemyBullet : BulletBase, INightInit
    { 
        public void Init()
        {
            MaxDistanceDelta = 0.4f;
            TargetTag = "Shelter";
            towardPoint = GameObject.FindWithTag("Shelter").transform;
        }

        protected override void DestroyEffect() => Effects.Instance.BlueBlow(transform.position, isWithAdditionalExp, true);
    }
}