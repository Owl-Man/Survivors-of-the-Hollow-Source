using System.Collections;
using NTC.Global.Pool;
using UnityEngine;

namespace Map
{
    public class BlockEffects : MonoBehaviour
    {
        [SerializeField] private GameObject[] blockParts;
        [SerializeField] private GameObject smallBlockPart;

        public static BlockEffects Instance;

        private void Awake() => Instance = this;

        public void StartBlockHitEffect(Vector3 position)
        {
            if (Random.Range(0, 6) != 0) return;

            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                StartCoroutine(DestroyPart(NightPool.Spawn(smallBlockPart, position, Quaternion.identity)));
            }
        }

        public void StartBlockDestroyEffect(Vector3 position) 
        {
            for (int i = 0; i < Random.Range(2, 5); i++)
            {
                StartCoroutine(DestroyPart(NightPool.Spawn(blockParts[Random.Range(0, blockParts.Length)], position, Quaternion.identity)));
            }
        }

        private IEnumerator DestroyPart(GameObject part) 
        {
            yield return new WaitForSeconds(Random.Range(3, 6));
            NightPool.Despawn(part);
        }
    }
}
