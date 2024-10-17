using System.Collections;
using NTC.Global.Pool;
using UnityEngine;

public class ObjectDespawning : MonoBehaviour
{
    [SerializeField] private float timeToDespawn;

    private void OnEnable() => StartCoroutine(Despawn());

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(timeToDespawn);
        NightPool.Despawn(gameObject);
    }

    private void OnDisable() => StopAllCoroutines();
}