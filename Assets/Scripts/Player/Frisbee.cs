using UnityEngine;
using UnityEngine.InputSystem;

public class Frisbee : MonoBehaviour
{
    [SerializeField] public float throwStrength = 3f;
    private Rigidbody2D rb;
    private Vector3 startLocalPosition;
    private Quaternion startLocalRotation; //Quaternion er enheden den bruger til at gemme rotation
    private bool isHoldingFrisbee = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startLocalPosition = transform.localPosition;
        startLocalRotation = transform.localRotation;
    }

    void Update()
    {
       if (Mouse.current.leftButton.wasPressedThisFrame && isHoldingFrisbee)
        {
            isHoldingFrisbee = false;
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            Vector2 retning = mouseWorldPosition - transform.position;

            transform.SetParent(null);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(retning.normalized * throwStrength, ForceMode2D.Impulse);
           
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
        isHoldingFrisbee = true;
    }
}