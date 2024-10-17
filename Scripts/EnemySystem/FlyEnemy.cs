using System;
using System.Collections;
using NTC.Global.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemySystem
{
    public class FlyEnemy : Enemy
    {
        [SerializeField] private GameObject fireBall;
        [SerializeField] private Transform shootPoint;
        private Transform _leftPoint, _rightPoint;

        private bool _isStay;

        public override void Init()
        {
            base.Init();
            
            TimeForChangeHpValue = 0.9f;

            GameObject[] points = GameObject.FindGameObjectsWithTag("FlyPoint");
            
            _leftPoint = points[0].transform;
            _rightPoint = points[1].transform;

            attackSpeed = 2f;

            Target = Random.Range(0, 2) == 0 ? _leftPoint.gameObject : _rightPoint.gameObject;
        }

        public override void Run()
        {
            if (!IsWalk || IsDead || IsAttack || _isStay) return;

            if (Math.Round(Math.Abs(Target.transform.position.x - transform.position.x)) == 0)
            {
                _isStay = true;
                StartCoroutine(ChangeTarget());
                return;
            }

            if (Rb.transform.position.y < 6) Rb.MovePosition(new Vector2(transform.position.x, transform.position.y + 0.1f));

            Move();
        }

        private IEnumerator ChangeTarget()
        {
            Rb.velocity = new Vector2(0, 0);
            Sr.flipX = Target != _leftPoint.gameObject;
            
            StartCoroutine(Attacking());

            yield return new WaitForSeconds(0.5f);
            
            SpriteRenderer fireballSprite = NightPool.Spawn(fireBall, shootPoint.position, new Quaternion(0, 0, 240, fireBall.transform.rotation.w)).GetComponent<SpriteRenderer>();
            fireballSprite.flipX = Target != _leftPoint.gameObject;
            
            yield return new WaitForSeconds(2f);
            
            Target = Target == _leftPoint.gameObject ? _rightPoint.gameObject : _leftPoint.gameObject;
            _isStay = false;
        }

        protected override void OnAttacking() => Animator.Play("Attack");

        protected override void OnDying()
        {
            base.OnDying();
            Rb.bodyType = RigidbodyType2D.Dynamic;
            Rb.freezeRotation = false;
            Rb.AddForce(new Vector2(Random.Range(-7f, 7f), Random.Range(-7f, 7f)), ForceMode2D.Impulse);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _isStay = false;
            Rb.freezeRotation = true;
            Rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}