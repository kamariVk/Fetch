using UnityEngine;
using UnityEngine.InputSystem;

public class Frisbee : MonoBehaviour
{
    [SerializeField] public float throwStrength = 3f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(Vector2.right * throwStrength, ForceMode2D.Impulse);
        }
    }
}