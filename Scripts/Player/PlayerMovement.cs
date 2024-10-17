using DataBase;
using NTC.Global.Cache;
using UnityEngine;

public class PlayerMovement : NightCache, INightRun, INightFixedRun
{
    [SerializeField] private float speed;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;
    
    public Rigidbody2D rb;

    [SerializeField] private GameObject drill;
    [SerializeField] private Transform[] drillStates;

    [SerializeField] private GameObject jetpackLightLeft, jetpackLightRight;
    [SerializeField] private ParticleSystem[] jetpackFire;

    public Joystick joystick;

    private Vector2 _moveInput, _moveVelocity;
    private static readonly int IsFlying = Animator.StringToHash("isFlying");

    private void Start()
    {
        if (DB.Access.gameData.chosenUpgrade == 0) speed += 0.7f;
    }

    public void FixedRun()
    {
        if (Player.Instance.BattleMode) return;

        rb.MovePosition(rb.position + _moveVelocity * Time.fixedDeltaTime);
    }

    public void Run()
    {
        if (Player.Instance.BattleMode) return;

        _moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        _moveVelocity = _moveInput.normalized * (speed + Improvements.Instance.CurrentSpeedBoost);

        if (_moveInput.x == 0)
        {
            animator.SetBool(IsFlying, false);
            
            jetpackLightLeft.SetActive(false);
            jetpackLightRight.SetActive(false);
        }
        else if (_moveInput.x > 0)
        {
            animator.SetBool(IsFlying, true);
            sprite.flipX = false;
            if (_moveInput.x < 0.7 && _moveInput.x > -0.7) Player.Instance.drillSR.flipY = false;

            if (DB.Access.GraphicsQuality != DB.LowGraphicsQuality)
            {
                StopAllJetpackFire();
                jetpackFire[0].Play();
            }
            
            jetpackLightLeft.SetActive(true);
            jetpackLightRight.SetActive(false);
            
            drill.transform.SetLocalPositionAndRotation(drillStates[0].transform.position, drillStates[0].transform.rotation);
        }
        else if (_moveInput.x < 0)
        {
            animator.SetBool(IsFlying, true);
            sprite.flipX = true;
            if (_moveInput.x < 0.7 && _moveInput.x > -0.7) Player.Instance.drillSR.flipY = false;

            if (DB.Access.GraphicsQuality != DB.LowGraphicsQuality)
            {
                StopAllJetpackFire();
                jetpackFire[1].Play();
            }

            jetpackLightLeft.SetActive(false);
            jetpackLightRight.SetActive(true);
            
            drill.transform.SetLocalPositionAndRotation(drillStates[1].transform.position, drillStates[1].transform.rotation);
        }

        if (joystick.Vertical > 0.7)
        {
            SetNormalDrillOrientation();

            if (DB.Access.GraphicsQuality != DB.LowGraphicsQuality)
            {
                StopAllJetpackFire();
                jetpackFire[3].Play();
            }

            drill.transform.SetLocalPositionAndRotation(drillStates[2].transform.position, drillStates[2].transform.rotation);
        }
        else if (joystick.Vertical < -0.7)
        {
            SetNormalDrillOrientation();

            if (DB.Access.GraphicsQuality != DB.LowGraphicsQuality)
            {
                StopAllJetpackFire();
                jetpackFire[2].Play();
            }

            drill.transform.SetLocalPositionAndRotation(drillStates[3].transform.position, drillStates[3].transform.rotation);
        }
    }

    private void SetNormalDrillOrientation() 
    {
        /*if (joystick.Horizontal < 0) Player.Instance.drillSR.flipY = false;
        else Player.Instance.drillSR.flipY = true;*/
    }

    private void StopAllJetpackFire()
    {
        for (int i = 0; i < jetpackFire.Length; i++)
        {
            jetpackFire[i].Stop();
        }
    }
}
