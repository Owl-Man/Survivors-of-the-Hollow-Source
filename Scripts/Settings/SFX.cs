using DataBase;
using UnityEngine;

public class SFX : MonoBehaviour
{
    private AudioSource _audioSource;

    public static SFX Instance;
    
    private void Awake() => Instance = this;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        UpdateState();
    }

    public void UpdateState() => _audioSource.volume = !DB.Access.IsSFXOn ? 0 : 0.2f;
}
