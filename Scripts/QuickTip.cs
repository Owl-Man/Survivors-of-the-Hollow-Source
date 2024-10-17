using UnityEngine;

public class QuickTip : MonoBehaviour
{
    [SerializeField] private GameObject qtPanel;

    private bool _isShowedByStart;
    
    private void Start()
    {
        if (PlayerPrefs.GetInt("IsQTShowen", 0) == 0)
        {
            qtPanel.SetActive(true);
            _isShowedByStart = true;
            PlayerPrefs.SetInt("IsQTShowen", 1);
            Time.timeScale = 0f;
        }
    }

    public void OnOKButtonClick()
    {
        if (_isShowedByStart)
        {
            Time.timeScale = 1f;
            _isShowedByStart = false;
        }
    }
}