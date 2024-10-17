using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnimation : MonoBehaviour
{
    [Header("Fading")] 
    
    [SerializeField] private float fadingSubTime = 0.01f;
    [SerializeField] private float fadingStep = 0.01f;
    [SerializeField] private bool isFadeAnimByFrame;
    
    [Header("Scaling")]
    
    [SerializeField] private bool isWithScale;
    [SerializeField] private float scaleStep = 0.02f;
    
    private Image _image;
    private TMP_Text _text;
    private CanvasGroup _canvasGroup;

    [SerializeField] private RectTransform rectTransform;

    private bool _isStop;

    private void Awake()
    { 
        TryGetComponent(out _canvasGroup);
        TryGetComponent(out _image);
        TryGetComponent(out _text);
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        if (_canvasGroup != null) StartCoroutine(StartFadedAnim(_canvasGroup));
        else if (_image != null) StartCoroutine(StartFadedAnim(_image));
        else if (_text != null) StartCoroutine(StartFadedAnim(_text));
    }

    private void OnDisable() => Stop();

    public void Stop() => StopAllCoroutines();
    public void StopWithReturn() => _isStop = true;

    public void EndFadeAnimText() => StartCoroutine(EndFadedAnim(_text));

    public void EndFadeAnimImage() => StartCoroutine(EndFadedAnim(_image));

    public void EndFadeAnimCanvasGroup()
    {
        StopCoroutine(EndFadedAnim(_canvasGroup));
        StartCoroutine(EndFadedAnim(_canvasGroup));
    }
    
    private YieldInstruction FadeYield() => isFadeAnimByFrame ? null : new WaitForSeconds(fadingSubTime);

    private IEnumerator StartFadedAnim(TMP_Text text)
    {
        if (isWithScale) StartCoroutine(UpScaling());
        
        text.alpha = 0;
        
        while (text.alpha < 1)
        {
            if (_isStop)
            {
                text.alpha = 1;
                _isStop = false;
                break;
            }
            
            text.alpha += fadingStep;
            
            yield return FadeYield();
        }
    }

    private IEnumerator StartFadedAnim(Image image)
    {
        if (isWithScale) StartCoroutine(UpScaling());
        
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        
        while (image.color.a < 1)
        {
            if (_isStop)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
                _isStop = false;
                break;
            }
            
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + fadingStep);
            
            yield return FadeYield();
        }
    }
    
    private IEnumerator StartFadedAnim(CanvasGroup canvasGroup)
    {
        if (isWithScale) StartCoroutine(UpScaling());
        
        canvasGroup.alpha = 0;
        
        while (canvasGroup.alpha < 1)
        {
            if (_isStop)
            {
                canvasGroup.alpha = 1;
                _isStop = false;
                break;
            }
            
            canvasGroup.alpha += fadingStep;
            
            yield return FadeYield();
        }
    }

    private IEnumerator UpScaling()
    {
        rectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        while (rectTransform.localScale.x < 1)
        {
            rectTransform.localScale = new Vector3(rectTransform.localScale.x + scaleStep, rectTransform.localScale.y + scaleStep, rectTransform.localScale.z + scaleStep);
            yield return null;
        }
    }
    
    private IEnumerator DownScaling()
    {
        rectTransform.localScale = new Vector3(1, 1, 1);

        while (rectTransform.localScale.x > 0)
        {
            rectTransform.localScale = new Vector3(rectTransform.localScale.x - scaleStep, rectTransform.localScale.y - scaleStep, rectTransform.localScale.z - scaleStep);
            yield return null;
        }
    }
    
    private IEnumerator EndFadedAnim(TMP_Text text)
    {
        if (isWithScale) StartCoroutine(DownScaling());
        
        while (text.alpha > 0)
        {
            if (_isStop)
            {
                text.alpha = 0;
                _isStop = false;
                break;
            }
            
            text.alpha -= fadingStep;
            
            yield return FadeYield();
        }
        
        gameObject.SetActive(false);
    }
    
    private IEnumerator EndFadedAnim(Image image)
    {
        if (isWithScale) StartCoroutine(DownScaling());
        
        while (image.color.a > 0)
        {
            if (_isStop)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
                _isStop = false;
                break;
            }
            
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - fadingStep);
            
            yield return FadeYield();
        }
        
        gameObject.SetActive(false);
    }
    
    private IEnumerator EndFadedAnim(CanvasGroup canvasGroup)
    {
        if (isWithScale) StartCoroutine(DownScaling());
        
        while (canvasGroup.alpha > 0)
        {
            if (_isStop)
            {
                canvasGroup.alpha = 0;
                _isStop = false;
                break;
            }
            
            canvasGroup.alpha -= fadingStep;
            
            yield return FadeYield();
        }
        
        gameObject.SetActive(false);
    }
}