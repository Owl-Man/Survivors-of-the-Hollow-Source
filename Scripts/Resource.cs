using System;
using NTC.Global.Cache;
using NTC.Global.Pool;
using System.Collections;
using Artifacts;
using DataBase;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Resource : NightCache, INightRun, INightInit
{
    [FormerlySerializedAs("m_SpringJoint")] [SerializeField] private SpringJoint2D mSpringJoint;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private string resourceName;

    private GameObject _target, _defaultTarget;
    private Rigidbody2D _targetRigidCache, _rb;

    private bool _isConnectedToPlayer;
    private bool _isConnectedToDrone;

    public void Init()
    {
        _defaultTarget = GameObject.FindGameObjectWithTag("Player");
        _target = _defaultTarget;

        _targetRigidCache = _target.GetComponent<Rigidbody2D>();
        mSpringJoint.connectedBody = _targetRigidCache;
    }

    public void Run()
    {
        if (_isConnectedToDrone || _isConnectedToPlayer) UpdateLineRenderer();

        if (mSpringJoint.enabled == false)
        {
            if (_isConnectedToPlayer) ConnectToPlayer();
            else if (_isConnectedToDrone) ConnectToDrone(mSpringJoint.connectedBody.gameObject);
        }

        if (_isConnectedToDrone)
        {
            if (Math.Round(Math.Abs(_target.transform.position.y -
                                    ResourceElevator.Instance.refiner.transform.position.y)) == 0)
            {
                Translate();
            }
        }
        
        _rb.velocity = new Vector2(CalculateVelocity(_rb.velocity.x), CalculateVelocity(_rb.velocity.y));
    }

    private void UpdateLineRenderer()
    {
        Vector3[] line = new Vector3[2];
        line[0] = transform.position;
        line[1] = _target.transform.position;

        lineRenderer.SetPositions(line);
    }

    private float CalculateVelocity(float directionVelocity)
    {
        if (Mathf.Abs(directionVelocity / Player.Instance.JetpackLoadCapacityFactor) < 0.02f) return 0.02f;

        if (Mathf.Abs(directionVelocity / Player.Instance.JetpackLoadCapacityFactor) < 0.08f)
            return directionVelocity / (Player.Instance.JetpackLoadCapacityFactor / 2);

        return directionVelocity / Player.Instance.JetpackLoadCapacityFactor;
    }

    public void ConnectToPlayer()
    {
        if (Player.Instance == null) return;

        CameraFocus.OnShelterEnter.RemoveListener(OnDisablePhysics);
        CameraFocus.OnShelterExit.RemoveListener(OnEnablePhysics);

        mSpringJoint.enabled = true;
        mSpringJoint.connectedBody = _targetRigidCache;
        _target = _defaultTarget;

        Player.Instance.inventory.AddResource(this);

        _isConnectedToPlayer = true;
    }

    public void ConnectToDrone(GameObject drone)
    {
        CameraFocus.OnShelterEnter.RemoveListener(OnDisablePhysics);
        CameraFocus.OnShelterExit.RemoveListener(OnEnablePhysics);
        
        mSpringJoint.enabled = true;
        mSpringJoint.connectedBody = drone.GetComponent<Rigidbody2D>();
        _target = drone;
        
        mSpringJoint.distance = 1;

        _rb.mass = 3;

        _isConnectedToDrone = true;
    }

    public void Disconnect()
    {
        mSpringJoint.enabled = false;
        _target = gameObject;
        
        CameraFocus.OnShelterEnter.AddListener(OnDisablePhysics);
        CameraFocus.OnShelterExit.AddListener(OnEnablePhysics);

        _rb.velocity = new Vector2(0.012f, 0.012f);

        _isConnectedToPlayer = false;
        
        UpdateLineRenderer();
    }

    private void OnEnable()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        
        _isConnectedToDrone = false;
        _isConnectedToPlayer = false;

        CameraFocus.OnShelterEnter.AddListener(TranslateResourceIntoRefiner);

        ConnectToPlayer();
        
        OnEnablePhysics();
        
        mSpringJoint.distance = 1.6f;
    }

    private void OnDisable()
    {
        CameraFocus.OnShelterEnter.RemoveListener(TranslateResourceIntoRefiner);
        
        CameraFocus.OnShelterEnter.RemoveListener(OnDisablePhysics);
        CameraFocus.OnShelterExit.RemoveListener(OnEnablePhysics);
    }

    public void TranslateResourceIntoRefiner()
    {
        if (!_isConnectedToPlayer) return;
        Translate();
    }
    
    private void Translate() => transform.position = new Vector2(0, -6.7f);

    public void OnEnablePhysics()
    {
        _rb.constraints = RigidbodyConstraints2D.None;
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.05f);
    }

    public void OnDisablePhysics() => _rb.constraints = RigidbodyConstraints2D.FreezeAll;


    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_isConnectedToPlayer && !_isConnectedToDrone && other.gameObject.CompareTag("ResourceDrone"))
        {
            ConnectToDrone(other.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Conversioner"))
        {
            mSpringJoint.enabled = true;
            mSpringJoint.connectedBody = collision.GetComponent<Rigidbody2D>();
            _target = collision.gameObject;
            mSpringJoint.distance = 0f;
            StartCoroutine(Conversion());
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (_isConnectedToDrone) return;
        
        if (col.gameObject.CompareTag("Player"))
        {
            ConnectToPlayer();
        }
    }

    private IEnumerator Conversion()
    {
        Player.Instance.inventory.RemoveResource(this);
        
        if (resourceName == "artifact")
        {
            ArtifactsManager.Instance.AddArtifactToActivateQueue();
        }
        else
        {
            int count = 1;

            if (DB.Access.gameData.chosenUpgrade == 2)
            {
                if (Random.Range(0, 2) == 0) count++;
            }

            yield return new WaitForSeconds(Random.Range(0.18f, 0.2f));

            if (resourceName == "gold") ResourcesManager.Instance.ChangeGoldValue(count);
            else if (resourceName == "raspberyl") ResourcesManager.Instance.ChangeRaspberylValue(count);
            else if (resourceName == "sapphire") ResourcesManager.Instance.ChangeSapphireValue(count);
        }

        NightPool.Despawn(gameObject);
    }
}
