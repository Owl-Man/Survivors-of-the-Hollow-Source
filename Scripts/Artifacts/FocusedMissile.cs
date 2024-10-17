using NTC.Global.Pool;
using UnityEngine;

namespace Artifacts
{
    public class FocusedMissile : MonoBehaviour
    {
        [SerializeField] private GameObject missile;

        public static FocusedMissile Instance;

        private void Awake() => Instance = this;

        public void Launch()
        {
            if (Random.Range(0, 3) == 0)
            {
                NightPool.Spawn(missile, transform.position);
            }
        }
    }
}