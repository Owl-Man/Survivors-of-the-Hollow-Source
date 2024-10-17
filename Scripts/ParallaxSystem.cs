using NTC.Global.Cache;
using UnityEngine;

public class ParallaxSystem : NightCache, INightRun, INightInit
{
    [SerializeField, Range(0f, 1f)] private float parallaxStrength = 0.1f;
    [SerializeField] private bool disableVerticalParallax;
    private Vector3 _targetPreviousPosition;
    
    private Transform _followingTarget;

    private bool _isParallaxActive;

    public void Init()
    {
        if (!_followingTarget) _followingTarget = Camera.main.transform;

        _targetPreviousPosition = _followingTarget.position;
        
        CameraFocus.OnShelterEnter.AddListener(OnEnableParallax);
        CameraFocus.OnShelterExit.AddListener(OnDisableParallax);
    }

    public void Run()
    {
        if (!_isParallaxActive) return;
        
        Vector3 delta = _followingTarget.position - _targetPreviousPosition;

        if (disableVerticalParallax) delta.y = 0;

        _targetPreviousPosition = _followingTarget.position;

        transform.position += delta * parallaxStrength;
    }

    public void OnEnableParallax() => _isParallaxActive = true;
    public void OnDisableParallax() => _isParallaxActive = false;
}