using EnemySystem;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TMP_Text score;

    private void Start()
    {
        Timer.Instance.isTimeGo = false;
        int minutes = (int)(Timer.Instance.passedTime / 60);
        int seconds = (int)(Timer.Instance.passedTime % 60);
        
        score.text = "<color=orange>" + Wave.Instance.GetCurrentWavesCount() + "</color> | "
                     + minutes + "m " + seconds + "s";
    }
}