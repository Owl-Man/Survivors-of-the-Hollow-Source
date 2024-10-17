using System.Collections;
using NTC.Global.Pool;
using UnityEngine;

namespace Artifacts
{
    public class Converter : MonoBehaviour
    {
        [SerializeField] private GameObject missile;

        public static Converter Instance;

        private void Awake() => Instance = this;

        public void FullLaunch() => StartCoroutine(Launching(Random.Range(2, 6)));
        
        public void Launch() => NightPool.Spawn(missile, transform.position);

        private IEnumerator Launching(int count)
        {
            for (int i = 0; i < count; i++)
            {
                NightPool.Spawn(missile, transform.position);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}