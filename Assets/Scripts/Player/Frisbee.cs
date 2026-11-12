using UnityEngine;
using UnityEngine.InputSystem;

public class Frisbee : MonoBehaviour
{
    [SerializeField] private float throwStrength = 3f;
    [SerializeField] private Vector3 holdOffset = new Vector3(0.758f, 0.045f, 0f);
    [SerializeField] private float pickupDelay = 0.3f;

    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private SpriteRenderer spriteRenderer;

    private bool isHolding = false;
    private Collider2D playerCollider;
    private InventoryController inventory;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (!isHolding) return;
        

        bool isSelected = inventory == null || inventory.GetSelectedItem() == gameObject;
        spriteRenderer.enabled = isSelected;

        if (isSelected && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Throw();
        }
    }

    void Throw()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = mousePos - transform.position;

        if (inventory != null) inventory.RemoveItem(gameObject);

        
        isHolding = false;
        spriteRenderer.enabled = true;
        transform.SetParent(null);
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        rigidbody.AddForce(direction.normalized * throwStrength, ForceMode2D.Impulse);

        Invoke(nameof(StopIgnoringPlayer), pickupDelay);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHolding) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PickUp(collision.transform);
        }
    }

    public void PickUp(Transform player)
    {
        HoldBy(player);
        if (inventory != null) inventory.AddItem(gameObject);
    }

    void HoldBy(Transform player)
    {
        playerCollider = player.GetComponent<Collider2D>();
        inventory = player.GetComponent<InventoryController>();

        //fjerner fysikken fra frisbeen
        isHolding = true;
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        rigidbody.linearVelocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;

        //sætter frisbeen i toms hånd
        transform.SetParent(player);
        transform.localPosition = holdOffset;
        transform.localRotation = Quaternion.identity;

        //ignorere collision på frisbeens hitbox og toms
        if (playerCollider != null) Physics2D.IgnoreCollision(collider, playerCollider, true);
    }
    
    void StopIgnoringPlayer()
    {
        if (isHolding) return;
        if (playerCollider != null) Physics2D.IgnoreCollision(collider, playerCollider, false);
    }
}