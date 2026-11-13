using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    Rigidbody2D rb;
    LineRenderer lr;
    DistanceJoint2D dj;
    public LayerMask grappleableLayer;
    public bool isGrappling = false;
    public MonoBehaviour movementScript;   // træk Player Movement herind
    public float swingForce = 20f;
    public float maxDistance = 6f;
    private InventoryController inventory;
    float swingDir;

    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        dj = GetComponent<DistanceJoint2D>();
        inventory = GetComponent<InventoryController>();

        dj.autoConfigureDistance = false;
        dj.autoConfigureConnectedAnchor = false;
        dj.enabled = false;
        lr.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            point.z = 0;
            

            if (Physics2D.OverlapCircle(point, 0.1f, grappleableLayer) != null)
            {
                GameObject selected = inventory.GetSelectedItem();
                if (selected == null || !selected.name.StartsWith("DogLeash")) return;
                if (Vector2.Distance(transform.position, point) > maxDistance) return;

                isGrappling = true;
                Debug.Log("GRAPPLER NU");


                dj.connectedAnchor = point;
                dj.distance = Vector2.Distance(transform.position, point);
                dj.enabled = true;
                swingDir = Mathf.Sign(point.x - transform.position.x);

                lr.enabled = true;
                if (movementScript != null) movementScript.enabled = false;
            }
        }

        if (Input.GetMouseButtonUp(1) && isGrappling)
        {
            isGrappling = false;
            lr.enabled = false;
            dj.enabled = false;
            if (movementScript != null) movementScript.enabled = true;
        }

        if (isGrappling)
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, dj.connectedAnchor);
        }
    }

    void FixedUpdate()
    {
        if (!isGrappling) return;

        Vector2 toAnchor = dj.connectedAnchor - rb.position;
        Vector2 tangent = Vector2.Perpendicular(toAnchor).normalized;
        if (Mathf.Sign(tangent.x) != swingDir) tangent = -tangent;

        // skub kun langs svinget, mens hun er under punktet
        if (rb.position.y < dj.connectedAnchor.y)
        {
            rb.AddForce(tangent * swingForce);
        }
    }
}