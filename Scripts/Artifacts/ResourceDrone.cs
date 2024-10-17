using System;
using NTC.Global.Cache;
using UnityEngine;

namespace Artifacts
{
    public class ResourceDrone : NightCache, INightFixedRun
    {
        [SerializeField] private bool isStartUpTarget;
        
        private ResourceElevator _resourceElevator;

        private Rigidbody2D _rb;
        private Transform _currentTarget;

        private void Start()
        {
            _resourceElevator = ResourceElevator.Instance;
            
            _rb = GetComponent<Rigidbody2D>();
            
            _currentTarget = isStartUpTarget ?
                _resourceElevator.upTargetDronePoint.transform : _resourceElevator.downTargetDronePoint.transform;
        }

        public void FixedRun()
        {
            if (Math.Round(Math.Abs(_currentTarget.position.y - transform.position.y)) == 0)
            {
                ChangeTarget();
                return;
            }

            transform.position = new Vector2(0, transform.position.y);
            
            if (_currentTarget == _resourceElevator.upTargetDronePoint.transform)
                _rb.MovePosition(new Vector2(transform.position.x, transform.position.y + 0.035f));
            else _rb.MovePosition(new Vector2(transform.position.x, transform.position.y - 0.035f));
        }

        private void ChangeTarget()
        {
            _currentTarget = _currentTarget == _resourceElevator.upTargetDronePoint.transform
                ? _resourceElevator.downTargetDronePoint.transform :
                _resourceElevator.upTargetDronePoint.transform;
        }
    }
}