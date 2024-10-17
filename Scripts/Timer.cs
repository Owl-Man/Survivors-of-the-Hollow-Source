using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float passedTime;
    public bool isTimeGo = true;
    
    public static Timer Instance;

    private void Awake() => Instance = this;

    private void Start() => StartCoroutine(Clocking());

    private IEnumerator Clocking()
    {
        while (isTimeGo)
        {
            yield return new WaitForSeconds(1f);
            passedTime += 1f;
        }
    }
}