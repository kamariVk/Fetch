using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    public Transform player;
    public float offsetX = 6f;          // hvor langt til højre for spilleren kameraet kigger
    public bool followY = false;        // slå til hvis kameraet også skal følge op/ned
    public float fixedY = 0f;           // kameraets højde når followY er slået fra

    [Header("Kant")]
    [Tooltip("Træk et objekt med en BoxCollider2D ind her. Kameraet viser aldrig noget udenfor boksen.")]
    public BoxCollider2D cameraBounds;

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player == null) return;

        float x = player.position.x + offsetX;
        float y = followY ? player.position.y : fixedY;

        if (cameraBounds != null)
        {
            Bounds b = cameraBounds.bounds;
            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;

            // Hvis boksen er mindre end kameraet, centreres kameraet bare i boksen
            x = (b.size.x <= halfWidth * 2) ? b.center.x : Mathf.Clamp(x, b.min.x + halfWidth, b.max.x - halfWidth);
            y = (b.size.y <= halfHeight * 2) ? b.center.y : Mathf.Clamp(y, b.min.y + halfHeight, b.max.y - halfHeight);
        }

        transform.position = new Vector3(x, y, -10);
    }
}
