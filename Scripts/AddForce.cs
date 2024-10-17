using UnityEngine;
using Random = UnityEngine.Random;

public class AddForce : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2d;

    private void Start() =>
        rigidbody2d.AddForce(new Vector2(Random.Range(-7f, 7f), Random.Range(-7f, 7f)), ForceMode2D.Impulse);
}
