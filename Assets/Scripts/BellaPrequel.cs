using UnityEngine;

public class BellaPrequel : MonoBehaviour
{
    [SerializeField] private float runSpeed = 9f;
    [SerializeField] private float stopDistance = 1f;
    [SerializeField] private Frisbee frisbee;
    [SerializeField] private bool spriteFacesLeft = true;
    private Animator animator;
    private SpriteRenderer spirteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spirteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        bool isThrown = frisbee.gameObject.activeInHierarchy && frisbee.transform.parent == null;
        if (!isThrown)
        {
            SetWalking(false);   // Tom har den: stå stille
            return;
        }


  
        float afstand = frisbee.transform.position.x - transform.position.x;

        if (Mathf.Abs(afstand) <= stopDistance) //Mathf.abs giver den aboslute værdi, så hvis x er -2 fx så laver den det om til 2
        {
            SetWalking(false);
            return;
        }

        Face(afstand > 0);
        SetWalking(true);
        Vector3 position = transform.position;
        position.x = Mathf.MoveTowards(position.x, frisbee.transform.position.x, runSpeed * Time.deltaTime); //Mathf.MoveTowards(start, mål, skridt)
        transform.position = position;   // kun vandret, hun bliver i samme højde
    }

    void Face(bool right)
    { 
        if (spriteFacesLeft)
        {
            spirteRenderer.flipX = right;
        }
        else
        {
            spirteRenderer.flipX = !right;
        }
    }

    void SetWalking(bool walking)
    {
        if (animator != null)
        animator.SetBool("isWalking", walking);
    }
}
