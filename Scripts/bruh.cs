using DataBase;
using TMPro;
using UnityEngine;

public class bruh : MonoBehaviour
{
    private void Start()
    {
        if (!DB.Access.IsBruhModeOn) return;

        GetComponent<TMP_Text>().text = "YOU BRUHED";
        GetComponent<AudioSource>().Play();
    }
}
