using System.Collections;
using DataBase;
using NTC.Global.Pool;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Effects : MonoBehaviour
{
    [SerializeField] private GameObject redBf, blueBf, hitEffect, bulletBlueFireEffect, bulletFireEffect, expEffect;
    [SerializeField] private ParticleSystem fireEffect;
    [SerializeField] private Light2D lightEffect;
        
    private float _lightIntensityCache;

    public static Effects Instance;

    private void Start()
    {
        Instance = this;
        _lightIntensityCache = lightEffect.intensity;
    }

    public void Shoot() => StartCoroutine(StartShootEffect());
    public void Hit(Vector2 position) => StartCoroutine(StartHitEffect(position));
    public void Light() => StartCoroutine(StartLightEffect());
    public void RedBlow(Vector2 position, bool isExp, bool isFire) => StartCoroutine(StartBlowEffect(position, redBf, isExp, isFire));
    public void BlueBlow(Vector2 position, bool isExp, bool isFire) => StartCoroutine(StartBlowEffect(position, blueBf, isExp, isFire));

    private IEnumerator StartShootEffect()
    {
        fireEffect.Play();
        
        lightEffect.gameObject.SetActive(true);
        StartCoroutine(StartLightEffect());
        
        yield return new WaitForSeconds(0.5f);
        
        StopCoroutine(StartLightEffect());
        lightEffect.gameObject.SetActive(false);
        lightEffect.intensity = _lightIntensityCache;
    }
    
    private IEnumerator StartHitEffect(Vector2 position)
    {
        GameObject effect = NightPool.Spawn(hitEffect, position, Quaternion.identity);
        effect.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.4f);
        NightPool.Despawn(effect);
    }

    private IEnumerator StartLightEffect()
    {
        while (lightEffect.intensity > 0 && lightEffect.gameObject.activeSelf)
        {
            lightEffect.intensity -= 0.5f;
            yield return null;
        }
    }

    private IEnumerator StartBlowEffect(Vector2 position, GameObject blowEffect, bool isExp, bool isFire)
    {
        GameObject additionalEffectCache = blowEffect;
        GameObject expEffectCache = blowEffect;

        if (DB.Access.GraphicsQuality != DB.LowGraphicsQuality)
        {
            if (isFire)
            {
                if (blowEffect == blueBf)
                    additionalEffectCache = NightPool.Spawn(bulletBlueFireEffect, position, Quaternion.identity);
                else additionalEffectCache = NightPool.Spawn(bulletFireEffect, position, Quaternion.identity);
            }

            if (isExp) expEffectCache = NightPool.Spawn(expEffect, position, Quaternion.identity);
        }

        if (blowEffect != redBf || (blowEffect == redBf && DB.Access.GraphicsQuality == DB.LowGraphicsQuality))
        {
            GameObject effect = NightPool.Spawn(blowEffect, position, Quaternion.identity);
            
            yield return new WaitForSeconds(0.15f);
        
            NightPool.Despawn(effect);
        }
        else yield return new WaitForSeconds(0.15f);
        
        if (DB.Access.GraphicsQuality != DB.LowGraphicsQuality && isFire)
        {
            yield return new WaitForSeconds(3.5f);
            NightPool.Despawn(additionalEffectCache);
        }

        yield return new WaitForSeconds(1);
        
        if (DB.Access.GraphicsQuality != DB.LowGraphicsQuality)
        {
            if (isExp) NightPool.Despawn(expEffectCache);
        }
    }
}