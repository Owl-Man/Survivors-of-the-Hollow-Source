using DataBase;
using UnityEngine;

public class DrillSound : MonoBehaviour
{
    [SerializeField] private AudioSource blockHitSource, blockDestroyedSource;

    public static DrillSound Instance;

    private void Awake() => Instance = this;

    public void OnBlockHit()
    {
        if (DB.Access.IsSFXOn) blockHitSource.Play();
    }

    public void OnBlockDestroy()
    {
        if (DB.Access.IsSFXOn) blockDestroyedSource.Play();
    }
}
