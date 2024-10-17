using UnityEngine;

namespace EnemySystem
{
    public class DefaultEnemy : Enemy
    {
        public override void Init()
        {
            base.Init();
            TimeForChangeHpValue = 0.3f;
        }

        public override void Run()
        {
            if (!IsWalk || IsDead || IsAttack) return;
            
            if (Mathf.Abs(Target.transform.position.x - transform.position.x) <= 5.02f)
            {
                IsWalk = false;
                if (!isEnemyWithSkeletonAnimation) Animator.SetBool(Walk, false);
            }
            
            Move();
        }
        
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!IsAttack && !IsDead && collision.CompareTag("Shelter"))
            {
                IsAttack = true;
                StartCoroutine(Attacking());
            }
        }
    }
}
