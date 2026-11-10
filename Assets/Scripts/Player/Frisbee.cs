using System.Collections;
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
    private Collider2D col;
    private Collider2D[] holderColliders = new Collider2D[0];
    [SerializeField] private float pickupDelay = 0.3f; // hvor længe efter kastet Tom ikke kan samle den op igen



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startLocalPosition = transform.localPosition;
        startLocalRotation = transform.localRotation;
        inventory = FindFirstObjectByType<InventoryController>();
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        // Mens Tom holder frisbeen, må den ikke støde ind i ham (ellers skubber den ham væk)
        if (transform.parent != null) IgnoreHolder(transform.parent, true);

    }

    void Update()
    {
        // Uden inventory i scenen (fx testscener) regnes frisbeen altid som valgt
        bool isSelected = inventory == null || inventory.GetSelectedItem() == gameObject;
        if (isHoldingFrisbee) sr.enabled = isSelected;

       if (Mouse.current.leftButton.wasPressedThisFrame && isHoldingFrisbee && isSelected)
        {
            isHoldingFrisbee = false;
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            Vector2 retning = mouseWorldPosition - transform.position;

            if (inventory != null) inventory.RemoveItem(gameObject);
            transform.SetParent(null);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(retning.normalized * throwStrength, ForceMode2D.Impulse);
        StartCoroutine(ReenableHolderCollision());
           
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
        IgnoreHolder(player, true);
        transform.localPosition = startLocalPosition;
        transform.localRotation = startLocalRotation;
        isHoldingFrisbee = true;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        InventoryController playerInventory = player.GetComponent<InventoryController>();
        if (playerInventory != null) inventory = playerInventory;
        if (inventory != null) inventory.AddItem(gameObject);
    }

    // Lad frisbeen og den, der holder den, ignorere hinanden (eller ikke)
    void IgnoreHolder(Transform holder, bool ignore)
    {
        if (col == null) return;
        if (ignore) holderColliders = holder.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D c in holderColliders)
        {
            if (c != null && c != col) Physics2D.IgnoreCollision(col, c, ignore);
        }
    }

    // Lidt efter kastet må Tom gerne røre frisbeen igen, så han kan samle den op
    IEnumerator ReenableHolderCollision()
    {
        yield return new WaitForSeconds(pickupDelay);
        if (!isHoldingFrisbee) IgnoreHolder(null, false);
    }
}
