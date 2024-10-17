using UnityEngine;

public class AdditionalUIZoneShow : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;

    private void OnTriggerEnter2D(Collider2D colliderEnter)
    {
        if (colliderEnter.CompareTag("Player"))
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D colliderExit)
    {
        if (colliderExit.CompareTag("Player"))
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(false);
            }
        }
    }
}
