using UnityEngine;
using UnityEngine.InputSystem;

public class Frisbee : MonoBehaviour
{
    [SerializeField] public float throwStrength = 3f;
    private Rigidbody2D rb;
    private Vector3 startLocalPosition;
    private Quaternion startLocalRotation; //Quaternion er enheden den bruger til at gemme rotation
    private bool isHoldingFrisbee = true;
    private InventoryController inventory;
    private SpriteRenderer sr;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startLocalPosition = transform.localPosition;
        startLocalRotation = transform.localRotation;
        inventory = FindFirstObjectByType<InventoryController>();
        sr = GetComponentInChildren<SpriteRenderer>();

    }

    void Update()
    {
        bool isSelected = inventory.GetSelectedItem() == gameObject;
        if (isHoldingFrisbee) sr.enabled = isSelected;

       if (Mouse.current.leftButton.wasPressedThisFrame && isHoldingFrisbee && isSelected)
        {
            isHoldingFrisbee = false;
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            Vector2 retning = mouseWorldPosition - transform.position;

            inventory.RemoveItem(gameObject);
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

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        inventory = player.GetComponent<InventoryController>();
        inventory.AddItem(gameObject);
    }
}