using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] controls;

    public static ControlPanel Instance;

    private void Awake() => Instance = this;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetControlButtonsActive(true);
            Improvements.Instance.CheckAnyBoostsAvailability();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) SetControlButtonsActive(false);
    }

    public void SetControlButtonsActive(bool state)
    {
        for (int i = 0; i < controls.Length; i++)
        {
            controls[i].SetActive(state);
        }
    }
}
