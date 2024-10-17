using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PulseAnimation : MonoBehaviour
{
    [SerializeField] private float step = 0.015f;
    [SerializeField] private bool isTimeScaled;
    private Image _image;

    private void Awake() => _image = GetComponent<Image>();

    private void OnEnable() => StartCoroutine(Pulse());

    private void OnDisable() => StopAllCoroutines();

    private IEnumerator Pulse()
    {
        while (true)
        {
            while (_image.color.a > 0.5)
            {
                _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a - step);
                
                if (isTimeScaled) yield return new WaitForSeconds(0.013f);
                else yield return new WaitForSecondsRealtime(0.013f);
            }

            if (isTimeScaled) yield return new WaitForSeconds(0.013f);
            else yield return new WaitForSecondsRealtime(0.013f);

            while (_image.color.a < 1)
            {
                _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a + step);
                
                if (isTimeScaled) yield return new WaitForSeconds(0.013f);
                else yield return new WaitForSecondsRealtime(0.013f);
            }
        }
    }
}