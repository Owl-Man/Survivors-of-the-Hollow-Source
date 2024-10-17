using System.Collections;
using Artifacts;
using GunSystem;
using NTC.Global.Cache;
using NTC.Global.Pool;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace EnemySystem
{
    //Main abstract class for any Enemy
    public abstract class Enemy : NightCache, INightRun, INightInit
    {
        [Header("Components")]
        
        [SerializeField] private Light2D light2D;
        
        protected Rigidbody2D Rb;
        protected SpriteRenderer Sr;
        protected Animator Animator;

        private AudioSource _hit;
        
        [Header("Skeleton")]

        [SerializeField] protected bool isEnemyWithSkeletonAnimation;

        private SkeletonAnimation _skeletonAnimation;
        
        private const string IdleAnimKey = "Idle";
        private const string WalkAnimKey = "Walk";
        private const string AttackAnimKey = "Attack";
        private const string DeadAnimKey = "Dead";
        
        [Header("Values")]

        [SerializeField] private Transform hitPoint, hitPointLeft;

        public float hp;
        public short damage;
        
        [SerializeField] protected float speed, attackSpeed;
        [SerializeField] protected bool isSpriteXFlip;
        
        protected static readonly int Walk = Animator.StringToHash("isWalk");
        protected static readonly int Attack = Animator.StringToHash("isAttack");

        protected GameObject Target;

        protected bool IsAttack, IsWalk = true, IsDead;
        
        protected float DefaultHp;
        private short _defaultDamage;
        
        private float _lightIntensityCache;

        protected float TimeForChangeHpValue;

        protected bool IsBlowHit;
        private bool _isLight2DNull;

        public abstract void Run();

        public virtual void Init()
        {
            Rb = GetComponent<Rigidbody2D>();
            TryGetComponent(out Sr);
            Animator = GetComponent<Animator>();
            
            _isLight2DNull = light2D == null;
            
            if (isEnemyWithSkeletonAnimation) _skeletonAnimation = GetComponent<SkeletonAnimation>();
                
            DefaultHp = hp;
            _defaultDamage = damage;

            _hit = GetComponent<AudioSource>();
            
            if (!_isLight2DNull) _lightIntensityCache = light2D.intensity;
            Target = GameObject.FindWithTag("Shelter");

            if (!isEnemyWithSkeletonAnimation) Animator.SetBool(Walk, true);
            else _skeletonAnimation.AnimationState.SetAnimation(0, WalkAnimKey, true);
        }

        protected void Move()
        {
            if (Target.transform.position.x > transform.position.x)
            {
                Rb.velocity = new Vector2(speed, 0);
                
                if (!isEnemyWithSkeletonAnimation) Sr.flipX = isSpriteXFlip;
                else
                    transform.rotation =
                        new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
                
                if (hitPointLeft != null) hitPoint = hitPointLeft;
            }
            else if (Target.transform.position.x < transform.position.x)
            {
                Rb.velocity = new Vector2(-speed, 0);

                if (!isEnemyWithSkeletonAnimation) Sr.flipX = !isSpriteXFlip;
                else
                    transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z,
                        transform.rotation.w);
            }
        }
        
        private void OnEnable()
        {
            if (isEnemyWithSkeletonAnimation)
                _skeletonAnimation.AnimationState.SetAnimation(0, WalkAnimKey, true);
        }

        protected virtual void OnDisable()
        {
            hp = DefaultHp;
            damage = _defaultDamage;

            IsWalk = true;
            IsAttack = false;
            IsDead = false;

            if (!isEnemyWithSkeletonAnimation)
            {
                Animator.SetBool(Walk, true);
                Animator.SetBool(Attack, false);
                Animator.Play("Idle");
            }

            if (!isEnemyWithSkeletonAnimation) Sr.color = new Color(Sr.color.r, Sr.color.g, Sr.color.b, 1);
            if (!_isLight2DNull) light2D.intensity = _lightIntensityCache;
            
            StopAllCoroutines();
        }

        //Events for child override
        
        protected virtual void OnHPChanging() {}
        
        protected virtual void OnDying()
        {
            if (!isEnemyWithSkeletonAnimation) Animator.Play("Die");
            else _skeletonAnimation.AnimationState.SetAnimation(0, DeadAnimKey, false);
        }
        
        protected virtual void OnAttacking()
        {
            if (!isEnemyWithSkeletonAnimation) Animator.Play(Random.Range(0, 2) == 0 ? "Attack" : "Attack2");
        }

        protected virtual void AfterAttack() {}
        
        //Damage taking
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Bullet")) return;

            TakeDamage(Gun.Instance.Damage, collision.transform.position);

            //Missile blow
            Effects.Instance.RedBlow(collision.transform.position, true, false);
            NightPool.Despawn(collision.gameObject);
        }

        public void TakeDamage(short value, Vector2 hitPosition)
        {
            if (ArtifactsManager.Instance.ArtifactsState.IsFocusedMissileActive)
                FocusedMissile.Instance.Launch();

            if (ArtifactsManager.Instance.ArtifactsState.IsProtectionPiercerActive &&
                (hp * 100) / DefaultHp > 90)
            {
                hp--;
                TextPopUp.Instance.CallPopUp(hitPosition, "-" + value, new Color(1f, 0.73f, 0.52f));
            }

            if (ArtifactsManager.Instance.ArtifactsState.IsConcentratedDamageActive &&
                Shelter.Instance.GetHpPercentage() < 20)
                value *= 2;

            if (ArtifactsManager.Instance.ArtifactsState.IsWeakPointActive &&
                Random.Range(0, 12) == 0)
                value *= 2;
            
            hp -= value;
            TextPopUp.Instance.CallPopUp(hitPosition, "-" + value, new Color(1f, 0.73f, 0.52f));
            OnHPChanging();
            
            if (hp <= 0 && !IsDead) StartCoroutine(Dying());
        }
        
        //Coroutines
        
        /*The moment of the attack is determined by the child class
        But here the basic attack behavior is defined*/
        protected IEnumerator Attacking()
        {
            IsAttack = true;
            
            if (!isEnemyWithSkeletonAnimation) Animator.SetBool(Attack, IsAttack);
            else _skeletonAnimation.AnimationState.SetAnimation(0, AttackAnimKey, false);
            
            OnAttacking();
            
            yield return new WaitForSeconds(TimeForChangeHpValue);
            
            AfterAttack();
            Shelter.Instance.ChangeHpValue(-damage);
            
            if (!isEnemyWithSkeletonAnimation) Animator.SetBool(Attack, false);

            if (!IsBlowHit)
            {
                if (hitPoint != null) Effects.Instance.Hit(hitPoint.position);
                _hit.Play();
            }
            else Effects.Instance.RedBlow(hitPoint.position, true, false);

            yield return new WaitForSeconds(attackSpeed);
            
            IsAttack = false;
            if (!isEnemyWithSkeletonAnimation) Animator.SetBool(Attack, IsAttack);
        }

        protected IEnumerator Dying()
        {
            IsDead = true;
            Wave.Instance.DecreaseEnemyCount();
            OnDying();

            if (ArtifactsManager.Instance.ArtifactsState.IsAegisActive) Shelter.Instance.ArtifactRepair();
            
            if (ArtifactsManager.Instance.ArtifactsState.IsPreonMergerActive)
            {
                if (Random.Range(0, 2) == 0)
                {
                    ResourcesManager.Instance.ChangeGoldValue(1);
                }
            }
            
            StartCoroutine(Disappearing());
            yield return new WaitForSeconds(1.75f);
            NightPool.Despawn(this);
        }

        protected IEnumerator Disappearing()
        {
            if (_isLight2DNull) yield break;
            yield return new WaitForSeconds(0.4f);

            if (!isEnemyWithSkeletonAnimation)
            {
                while (Sr.color.a > 0)
                {
                    Sr.color = new Color(Sr.color.r, Sr.color.g, Sr.color.b, Sr.color.a - 0.01f);

                    if (light2D.intensity > 0) light2D.intensity -= 0.1f;

                    yield return null;
                }
            }
            else
            {
                while (light2D.intensity > 0)
                {
                    light2D.intensity -= 0.1f;
                    yield return null;
                }
            }
        }
    }
}