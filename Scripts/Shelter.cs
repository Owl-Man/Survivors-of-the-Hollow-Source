using System.Collections;
using System.Globalization;
using Artifacts;
using DataBase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shelter : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject brokenGlass, repairShelterBtn;

    [SerializeField] private ParticleSystem repairEffect;
    
    [SerializeField] private FadeAnimation damageOutline;
    
    [SerializeField] private Image hpBar;
    [SerializeField] private TMP_Text hpText;
    
    public CameraShake cameraShake;
    
    [SerializeField] private float hp;
    private float _defaultHp = 100;

    private bool _isDamageOutlineShowing;

    public static Shelter Instance;

    private void Awake() => Instance = this;

    private void Start()
    {
        if (DB.Access.gameData.chosenUpgrade == 1) _defaultHp += 30;

        hp = _defaultHp;
        
        UpdateValues();
    }

    public void IncreaseMaxHp()
    {
        _defaultHp += 20;
        ChangeHpValue(20);
    }

    public void ChangeHpValue(int value)
    {
        if (hp <= 0) return;
        hp += value;
        
        if (!_isDamageOutlineShowing) StartCoroutine(ShowDamage());
        
        UpdateValues();
        cameraShake.Shake();
        
        if (hp > _defaultHp) hp = _defaultHp;
    }

    private IEnumerator ShowDamage()
    {
        _isDamageOutlineShowing = true;
        damageOutline.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        damageOutline.EndFadeAnimImage();
        _isDamageOutlineShowing = false;
    }

    public void RepairShelter()
    {
        if (hp >= _defaultHp || ResourcesManager.Instance.SapphireCount < 1) return;
        
        if (ArtifactsManager.Instance.ArtifactsState.IsConverterActive) Converter.Instance.FullLaunch();

        ResourcesManager.Instance.ChangeSapphireValue(-1);
        ChangeHpValue(40);
    }

    public void ArtifactRepair()
    {
        ChangeHpValue(2);
        
        TextPopUp.Instance.CallPopUp(new Vector2(transform.position.x + Random.Range(-1f, 1f),
            transform.position.y + Random.Range(-1f, 1f)), "+2", new Color(0.53f, 0.78f, 1f));
        
        if (ArtifactsManager.Instance.ArtifactsState.IsConverterActive) Converter.Instance.Launch();
        
        StopCoroutine(ShowingArtifactRepairEffect());
        StartCoroutine(ShowingArtifactRepairEffect());
    }

    private IEnumerator ShowingArtifactRepairEffect()
    {
        repairEffect.gameObject.SetActive(true);
        repairEffect.Stop();
        repairEffect.Play();
        yield return new WaitForSeconds(4);
        repairEffect.Stop();
        repairEffect.gameObject.SetActive(false);
    }

    private void UpdateValues()
    {
        if (hp <= 0)
        {
            hp = 0;
            gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }

        hpBar.fillAmount = (hp * 100) / _defaultHp / 100;
        hpText.text = hp.ToString(CultureInfo.CurrentCulture);

        repairShelterBtn.SetActive(hpBar.fillAmount <= 0.25f);

        brokenGlass.SetActive(hpBar.fillAmount <= 0.25f);
    }

    public float GetHpPercentage() => hpBar.fillAmount * 100;
}
