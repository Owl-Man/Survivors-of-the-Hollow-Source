using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CameraFocus : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject playerLight, drill;

    [SerializeField] private float focusingSpeed, increaseCameraSize;
    private float _defaultCameraSize;

    public static UnityEvent OnShelterEnter = new UnityEvent();
    public static UnityEvent OnShelterExit = new UnityEvent();

    private void Start() => _defaultCameraSize = mainCamera.orthographicSize;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerLight.SetActive(false);
            drill.SetActive(false);
            
            OnShelterEnter.Invoke();
            
            StopAllCoroutines();
            StartCoroutine(IncreaseCameraSize());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerLight.SetActive(true);
            drill.SetActive(true);
            
            OnShelterExit.Invoke();
            
            StopAllCoroutines();
            StartCoroutine(DecreaseCameraSize());
        }
    }

    private IEnumerator IncreaseCameraSize() 
    {
        while (mainCamera.orthographicSize < increaseCameraSize) 
        {
            mainCamera.orthographicSize += 0.1f;
            yield return new WaitForSeconds(focusingSpeed);
        }
    }

    private IEnumerator DecreaseCameraSize()
    {
        while (mainCamera.orthographicSize > _defaultCameraSize)
        {
            mainCamera.orthographicSize -= 0.1f;
            yield return new WaitForSeconds(focusingSpeed);
        }
    }
}
