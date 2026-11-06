using UnityEngine;

public class DeathZone : MonoBehaviour
{

    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.position = respawnPoint.position;

        }
    }
}
