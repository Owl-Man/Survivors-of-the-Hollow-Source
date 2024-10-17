using NTC.Global.Cache;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraControl : NightCache, INightFixedRun
{
    [FormerlySerializedAs("_player")] [SerializeField] private Transform player;

    [SerializeField] private float dumping = 1.5f;
    [SerializeField] private Vector2 offset = new Vector2(2f, 1f);

    [SerializeField] private bool hasLimit = true;
    [SerializeField] private float leftLimit, rightLimit, bottomLimit, upperLimit;

    private Vector3 _target;
    private int _lastX, _currentX;
    private bool _isLeft;

    private void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
        FocusOnPlayer(_isLeft);
    }

    public void FixedRun()
    {
        _currentX = Mathf.RoundToInt(player.position.x);

        if (_currentX > _lastX) _isLeft = false;
        else if (_currentX < _lastX) _isLeft = true;

        _lastX = Mathf.RoundToInt(player.position.x);

        _target = _isLeft
            ? new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z)
            : new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

        Vector3 currentPosition = Vector3.Lerp(transform.position, _target, dumping * Time.deltaTime);

        transform.position = currentPosition;

        if (hasLimit) 
        {
            transform.position = new Vector3
            (
                Mathf.Clamp(transform.position.x, leftLimit * -1, rightLimit),
                Mathf.Clamp(transform.position.y, bottomLimit * -1, upperLimit),
                transform.position.z
            );
        }
    }

    public void FocusOnPlayer(bool playerIsLeft)
    {
        _lastX = Mathf.RoundToInt(player.position.x);

        transform.position = playerIsLeft
            ? new Vector3(player.position.x - offset.x, player.position.y - offset.y, transform.position.z)
            : new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
    }
}