using UnityEngine;

namespace Artifacts
{
    public class DownDroneTargetLimitScript : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private CircleCollider2D _circleCollider2D;
        private RigidbodyConstraints2D _defaultRbConstraints2D;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
            
            _defaultRbConstraints2D = _rb.constraints;
            
            OnDisablePhysics();
            
            CameraFocus.OnShelterEnter.AddListener(OnDisablePhysics);
            CameraFocus.OnShelterExit.AddListener(OnEnablePhysics);
        }


        public void OnEnablePhysics()
        {
            _rb.constraints = _defaultRbConstraints2D;
            _circleCollider2D.enabled = true;
        }

        public void OnDisablePhysics()
        {
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _circleCollider2D.enabled = false;
        }
    }
}