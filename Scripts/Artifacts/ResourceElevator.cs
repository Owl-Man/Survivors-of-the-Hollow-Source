using System.Collections;
using UnityEngine;

namespace Artifacts
{
    public class ResourceElevator : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;

        [HideInInspector] public Transform refiner;
        
        public GameObject upTargetDronePoint, downTargetDronePoint;

        public static ResourceElevator Instance;

        private float _minY;
        
        private void Awake() => Instance = this;

        private void Start()
        {
            refiner = GameObject.FindWithTag("Conversioner").transform;
            StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (true)
            {
                if (downTargetDronePoint.transform.position.y < _minY)
                {
                    _minY = downTargetDronePoint.transform.position.y;
                    
                    Vector3[] line = new Vector3[2];
                    line[0] = refiner.position;
                    line[1] = downTargetDronePoint.transform.position;

                    lineRenderer.SetPositions(line);
                }

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}