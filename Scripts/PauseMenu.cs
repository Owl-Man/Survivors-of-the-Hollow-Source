using System.Collections;
using Google_ADS_System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void OnRestartButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void OnPauseClick() => StartCoroutine(ChangeGameTimeTo(0));

    public void OnResumeClick() => Time.timeScale = 1;

    private IEnumerator ChangeGameTimeTo(int time)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = time;
    }

    public void OnMenuButtonClick()
    {
        Time.timeScale = 1;
        if (Random.Range(0, 7) == 0) InterAd.Instance.ShowAd();
        SceneManager.LoadScene("Menu");
    }
}
