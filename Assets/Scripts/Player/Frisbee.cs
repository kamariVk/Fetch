using UnityEngine;
using UnityEngine.InputSystem;

public class Frisbee : MonoBehaviour
{
    [SerializeField] public float throwStrength = 3f;
    private Rigidbody2D rb;
    private Vector3 startLocalPosition;
    private Quaternion startLocalRotation; //Quaternion er enheden den bruger til at gemme rotation

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startLocalPosition = transform.localPosition;
        startLocalRotation = transform.localRotation;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(Vector2.right * throwStrength, ForceMode2D.Impulse);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PickUp(collision.transform);
        }
    }

    public void PickUp(Transform player)
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        transform.SetParent(player);
        transform.localPosition = startLocalPosition;
        transform.localRotation = startLocalRotation;
    }
}