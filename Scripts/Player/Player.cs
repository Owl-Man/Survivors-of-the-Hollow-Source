using System.Collections;
using DataBase;
using GunSystem;
using NTC.Global.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject drillLight, disableBattleButton;
    [SerializeField] private ParticleSystem drillPS;
    [SerializeField] private PlayerMovement pM;
    [SerializeField] private Gun gun;

    public PlayerInventory inventory;

    [SerializeField] private Sprite[] drills;
    public SpriteRenderer drillSR;

    [SerializeField] private GameObject[] drillAdditionalEffects;

    public float JetpackLoadCapacityFactor { get; private set; } = 5;

    public short DrillPower { get; private set; } = 1;
    private ushort _drillID;

    public bool BattleMode { get; private set; }
    private bool _isDrillWork;

    public static Player Instance;

    private void Awake() => Instance = this;

    private void Start()
    {
        if (DB.Access.gameData.chosenUpgrade == 4) DrillPower += 2;
    }

    public void IncreaseJetpackLoadCapacity() => JetpackLoadCapacityFactor -= 1;

    public void ActivateBattleMode() 
    {
        BattleMode = true;

        pM.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
        ControlPanel.Instance.SetControlButtonsActive(false);
        disableBattleButton.SetActive(true);
        pM.joystick.gameObject.SetActive(false);
        gun.controlButtons.SetActive(true);
    }

    public void DisableBattleMode() 
    {
        BattleMode = false;

        pM.rb.constraints = RigidbodyConstraints2D.None;
        pM.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        ControlPanel.Instance.SetControlButtonsActive(true);
        disableBattleButton.SetActive(false);
        pM.joystick.gameObject.SetActive(true);
        gun.controlButtons.SetActive(false);
    }

    public void UpgradeDrill() 
    {
        _drillID++;
        drillSR.sprite = drills[_drillID];
        DrillPower += 2;
    }

    public void DrillWork()
    {
        if (!_isDrillWork) StartCoroutine(StartDrillPSEffect());
    }

    private IEnumerator StartDrillPSEffect()
    {
        if (DB.Access.GraphicsQuality != DB.LowGraphicsQuality)
        {
            for (int i = 0; i < drillAdditionalEffects.Length; i++)
            {
                if (Random.Range(0, 2) == 0)
                {
                    NightPool.Spawn(drillAdditionalEffects[i], drillLight.gameObject.transform.position,
                        drillAdditionalEffects[i].transform.rotation);
                }
            }
        }
        
        _isDrillWork = true;
        if (DB.Access.GraphicsQuality == DB.LowGraphicsQuality) drillPS.Play();
        drillLight.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        drillLight.SetActive(false);
        if (DB.Access.GraphicsQuality == DB.LowGraphicsQuality) drillPS.Stop();
        _isDrillWork = false;
    }
}
