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
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(Physics2D.OverlapCircle(point, 0.1f, grappleableLayer) != null)
            {
                isGrappling = true;
                lr.enabled = true;
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, point);

                dj.enabled = true;
                dj.anchor = point;
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            isGrappling = false;
            lr.enabled = false;
            dj.enabled = false;
        }
        if(isGrappling)
        {
            lr.enabled = true;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
