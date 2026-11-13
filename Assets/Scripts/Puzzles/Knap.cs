using UnityEngine;

public class Knap : MonoBehaviour
{
    [SerializeField] private GameObject closedDoor;
    [SerializeField] private GameObject openDoor;

    void OnCollisionEnter2D(Collision2D collision)
    {
        closedDoor.SetActive(false);
        openDoor.SetActive(true);
    }
}
