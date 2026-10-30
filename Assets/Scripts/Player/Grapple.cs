using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    Rigidbody2D rb; 
    LineRenderer lr;
    DistanceJoint2D dj;
    public LayerMask grappleableLayer;
    public bool isGrappling = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        dj = GetComponent<DistanceJoint2D>();
        dj.enabled = false;
        lr.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(Physics2D.OverlapCircle(point, 0.1f, grappleableLayer) != null)
            {
                isGrappling = true;
                lr.enabled = true;
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, point);

                dj.connectedAnchor = point;
                dj.distance = Vector2.Distance(transform.position, point);
                dj.enabled = true;
                rb.AddForce(new Vector2(Mathf.Sign(point.x - transform.position.x) * 15f, 5f), ForceMode2D.Impulse);
            }
        }
        if(Input.GetMouseButtonUp(1))
        {
            isGrappling = false;
            lr.enabled = false;
            dj.enabled = false;
        }
        if(isGrappling)
        {
            lr.enabled = true;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
