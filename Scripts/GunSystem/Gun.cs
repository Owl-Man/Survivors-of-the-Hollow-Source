using System.Collections;
using Artifacts;
using DataBase;
using NTC.Global.Pool;
using UnityEngine;

namespace GunSystem
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Transform shootPoint;
        [SerializeField] private GameObject bullet, leftLimit, rightLimit;

        [SerializeField] private float step = 2f, speed = 1f, reloadTime = 0.5f;
        public short Damage { get; private set; }  = 1;

        public GameObject controlButtons;

        private SpriteRenderer _spriteRenderer;
        private AudioSource _shootSound;
        
        private BulletCountSystem _bulletCountSystem;

        private bool _isLeftLimitReached, _isRightLimitReached, _isGunReloaded = true;

        public static Gun Instance;

        private void Awake() => Instance = this;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _shootSound = GetComponent<AudioSource>();
            
            _bulletCountSystem = GetComponent<BulletCountSystem>();

            if (DB.Access.gameData.chosenUpgrade == 3) Damage += 2;
        }

        public void IncreaseBulletPower() => Damage += 2;
    
        public void IncreaseRotationSpeed() => speed += 0.25f;

        public void IncreaseReloadingSpeed() => reloadTime -= 0.1f;

        public void Shoot()
        {
            if (_bulletCountSystem.BulletCount == 0) return;
            
            if (_isGunReloaded == false) return;
        
            _isGunReloaded = false;
            
            if (ArtifactsManager.Instance.ArtifactsState.IsConcentratedShotsActive &&
                Shelter.Instance.GetHpPercentage() > 93)
                StartCoroutine(Reloading(reloadTime / 2));
            else 
                StartCoroutine(Reloading(reloadTime));
            
            _bulletCountSystem.UseBullet();
            if (DB.Access.GraphicsQuality != DB.LowGraphicsQuality) Effects.Instance.Shoot();
            if (DB.Access.IsSFXOn) _shootSound.Play();
            NightPool.Spawn(bullet, shootPoint.position, shootPoint.rotation);
        }

        private IEnumerator Reloading(float time)
        {
            yield return new WaitForSeconds(time);
            _isGunReloaded = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == leftLimit) _isLeftLimitReached = true;
            else if (collision.gameObject == rightLimit) _isRightLimitReached = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject == leftLimit) _isLeftLimitReached = false;
            else if (collision.gameObject == rightLimit) _isRightLimitReached = false;
        }

        public void MoveLeft() 
        {
            if (!_isLeftLimitReached)
            {
                transform.Rotate(0, 0, step * speed);

                UpdateSystem();
            }
        }

        public void MoveRight() 
        {
            if (!_isRightLimitReached)
            {
                transform.Rotate(0, 0, -step * speed);

                UpdateSystem();
            }
        }

        private void UpdateSystem()
        {
            if (transform.rotation.z <= 0.75f && transform.rotation.z > -0.6f) _spriteRenderer.flipY = false;
            else _spriteRenderer.flipY = true;
        }
    }
}
