using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;

    void Update()
    {

        float halfWidth = Camera.main.orthographicSize * Camera.main.aspect; //Find skærmens halve bredde

        float rightEdge = transform.position.x + halfWidth; //Find højre kant af skærmen

        float leftEdge = transform.position.x - halfWidth; //Find venstre kant af skærmen

        if (player.position.x > rightEdge)
        {
            transform.position += new Vector3(halfWidth * 2,0, 0);
        }
        if (player.position.x < leftEdge)
        {
            transform.position -= new Vector3(halfWidth * 2, 0, 0);
        }
    }
}