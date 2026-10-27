using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    private Rigidbody2D rigidbody;
    private int moveDirection = 0;
    public InputAction jumpAction;
    public float jumpForce;
    public bool isGrounded;
    public Transform groundCheck;
    public float checkRadius = 1f;
    public LayerMask groundLayer;
    public float jumpCutmultiplier;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        jumpAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = 0;
        if (Keyboard.current.aKey.isPressed)
        {
            
            moveDirection--;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            
            moveDirection++;
        }
        float horizontalSpeed = moveDirection * movementSpeed;
        rigidbody.linearVelocity = new Vector2(horizontalSpeed, rigidbody.linearVelocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (jumpAction.triggered && isGrounded)
        {
            Jump();
        }

        if (jumpAction.WasReleasedThisFrame() && rigidbody.linearVelocity.y > 0)
        {
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, rigidbody.linearVelocity.y* jumpCutmultiplier);
        }
    }


    public void Jump()
    {
        rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, jumpForce);
    }
}
