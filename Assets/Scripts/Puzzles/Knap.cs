using UnityEngine;

public class Knap : MonoBehaviour
{
    [SerializeField] private GameObject door;

    void OnCollisionEnter2D(Collision2D collision)
    {
        door.SetActive(false);
    }
}
