using System.Collections;
using TMPro;
using UnityEngine;

namespace GunSystem
{
    public class BulletCountSystem : MonoBehaviour
    {
        [SerializeField] private TMP_Text bulletCountText;
        [SerializeField] private GameObject goldRefreshButton, raspberylRefreshButton;
        public int BulletCount { get; private set; } = 20;
        
        private int _bulletCountDefault;

        private void Start()
        {
            _bulletCountDefault = BulletCount;
            UpdateUI();
        }

        public void RefreshBulletCountGold()
        {
            if (ResourcesManager.Instance.GoldCount < 2) return;
            
            ResourcesManager.Instance.ChangeGoldValue(-2);
            RefreshBulletCount();
        }
        
        public void RefreshBulletCountRaspberyl()
        {
            if (ResourcesManager.Instance.RaspberylCount < 1) return;
            
            ResourcesManager.Instance.ChangeRaspberylValue(-1);
            RefreshBulletCount();
        }

        private void RefreshBulletCount()
        {
            BulletCount = _bulletCountDefault;
            goldRefreshButton.SetActive(false);
            raspberylRefreshButton.SetActive(false);
            UpdateUI();
        }

        public void UseBullet()
        {
            BulletCount--;

            if (BulletCount == 0)
            {
                goldRefreshButton.SetActive(true);
                raspberylRefreshButton.SetActive(true);
            }
                
            UpdateUI();
        }

        private void UpdateUI()
        {
            bulletCountText.text = "x" + BulletCount;
            StopAllCoroutines();
            StartCoroutine(UpscaleAnimation());
        }

        private IEnumerator UpscaleAnimation()
        {
            while (bulletCountText.transform.localScale.x <= 1.35f)
            {
                bulletCountText.transform.localScale = new Vector3(bulletCountText.transform.localScale.x + 0.01f,
                    bulletCountText.transform.localScale.y + 0.01f, 1f);
                yield return new WaitForSeconds(0.01f);
            }

            while (bulletCountText.transform.localScale.x > 1.3f)
            {
                bulletCountText.transform.localScale = new Vector3(bulletCountText.transform.localScale.x - 0.01f,
                    bulletCountText.transform.localScale.y - 0.01f, 1f);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}