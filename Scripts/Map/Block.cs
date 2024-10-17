using System.Collections;
using System.Collections.Generic;
using DataBase;
using NTC.Global.Pool;
using UnityEngine;

namespace Map
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoxCollider2D reboundCollider, phCollider, triggerCollider;
        [SerializeField] private GameObject resource;

        [SerializeField] private short strength;
        private short _defaultStrength;

        [SerializeField] private Sprite[] hittedBlock;

        private bool _isHitted;

        public List<Block> blockNeighbourCache;

        private Player _player;

        private void Start()
        {
            _defaultStrength = strength;
            _player = Player.Instance;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Drill")) 
            {
                if (_isHitted) return;

                _isHitted = true;

                strength -= _player.DrillPower;
                
                _player.DrillWork();
                DrillSound.Instance.OnBlockHit();
                
                if (DB.Access.GraphicsQuality != DB.LowGraphicsQuality)
                    BlockEffects.Instance.StartBlockHitEffect(Player.Instance.transform.position);
                
                HitBlock();
                StartCoroutine(Rebound());
            }
        }

        private IEnumerator Rebound() 
        {
            yield return new WaitForSeconds(0.01f);
            reboundCollider.enabled = true;
            yield return new WaitForSeconds(0.055f);
            reboundCollider.enabled = false;
            _isHitted = false;

            if (strength <= 0)
            {
                if (resource != null)
                {
                    if (resource.name == "artifact")
                    {
                        NightPool.Spawn(resource, transform.position, Quaternion.identity);
                    }
                    else
                    {
                        for (int i = 0; i < Random.Range(2, 5); i++)
                        {
                            NightPool.Spawn(resource, transform.position, Quaternion.identity);
                        }
                    }
                }

                DestroyBlock();
            }
        }

        private void HitBlock() 
        {
            if (hittedBlock.Length == 0) return;

            int percentOfStrength = (strength * 100) / _defaultStrength;

            if (gameObject.name == "g1+" || gameObject.name == "g2+" || gameObject.name == "g3+")
            {
                if (percentOfStrength <= 45 && percentOfStrength >= 40) spriteRenderer.sprite = hittedBlock[0];
                else if (percentOfStrength >= 30) spriteRenderer.sprite = hittedBlock[1];
                else if (percentOfStrength >= 15) spriteRenderer.sprite = hittedBlock[2];
                else if (percentOfStrength >= 10) spriteRenderer.sprite = hittedBlock[3];
            }
            else 
            {
                if (percentOfStrength >= 85) spriteRenderer.sprite = hittedBlock[0];
                else if (percentOfStrength >= 55) spriteRenderer.sprite = hittedBlock[1];
                else if (percentOfStrength >= 25) spriteRenderer.sprite = hittedBlock[2];
            }
        }

        private void DestroyBlock()
        {
            CameraFocus.OnShelterEnter.RemoveListener(DisableColliders);
            CameraFocus.OnShelterExit.RemoveListener(EnableColliders);
            
            DrillSound.Instance.OnBlockDestroy();
            
            spriteRenderer.sprite = MapGenerator.Instance.GetBackground();

            for (int i = 0; i < blockNeighbourCache.Count; i++)
            {
                if (blockNeighbourCache[i] != null) blockNeighbourCache[i].BlockEnable();
            }

            BlockEffects.Instance.StartBlockDestroyEffect(transform.position);

            Destroy(reboundCollider);
            Destroy(phCollider);
            Destroy(triggerCollider);
            Destroy(this);
        }

        public void BlockEnable() 
        {
            CameraFocus.OnShelterEnter.AddListener(DisableColliders);
            CameraFocus.OnShelterExit.AddListener(EnableColliders);
            
            spriteRenderer.enabled = true;
            EnableColliders();
        }

        public void BlockDisable()
        {
            spriteRenderer.enabled = false;
            DisableColliders();
        }

        public void EnableColliders()
        {
            StopCoroutine(EnablingColliders());
            StartCoroutine(EnablingColliders());
        }

        private IEnumerator EnablingColliders()
        {
            phCollider.enabled = true;
            yield return null;
            triggerCollider.enabled = true;
        }

        public void DisableColliders()
        {
            phCollider.enabled = false;
            triggerCollider.enabled = false;
        }
    }
}
