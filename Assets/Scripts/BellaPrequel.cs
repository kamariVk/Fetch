using UnityEngine;

public class BellaPrequel : MonoBehaviour
{
    [SerializeField] private float runSpeed = 9f;
    [Tooltip("Hvor tæt på frisbeen hun stopper (vandret).")]
    [SerializeField] private float stopDistance = 1f;

    [Tooltip("Findes automatisk, hvis feltet er tomt.")]
    [SerializeField] private Frisbee frisbee;

    [Tooltip("Slå til, hvis Bella-tegningen kigger mod venstre (det gør crayon-Bella).")]
    [SerializeField] private bool spriteFacesLeft = true;

    private Animator animator;
    private SpriteRenderer sr;

    void Awake()
    {
        // Bella skal kunne ramme triggere (fx scene-skiftet). Det kræver en collider og en Rigidbody2D.
        // Kinematic + trigger = hun mærker triggere, men skubber ikke til noget og falder ikke.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        Collider2D col = GetComponent<Collider2D>();
        if (col == null) col = gameObject.AddComponent<BoxCollider2D>();   // får automatisk sprite'ens størrelse
        col.isTrigger = true;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();

        // Include = find den også, hvis den er slået fra, når scenen starter
        if (frisbee == null) frisbee = FindFirstObjectByType<Frisbee>(FindObjectsInactive.Include);
    }

    void Update()
    {
        if (frisbee == null)
        {
            SetWalking(false);   // frisbeen er væk (fx slettet af scene-skiftet)
            return;
        }

        // Frisbeen er kastet, når den er aktiv og ingen holder den (ingen forælder)
        bool isThrown = frisbee.gameObject.activeInHierarchy && frisbee.transform.parent == null;
        if (!isThrown)
        {
            SetWalking(false);   // Tom har den: stå stille
            return;
        }

        float dx = frisbee.transform.position.x - transform.position.x;
        if (Mathf.Abs(dx) <= stopDistance)
        {
            SetWalking(false);   // fremme ved frisbeen: vent på Tom
            return;
        }

        Face(dx > 0);
        SetWalking(true);
        Vector3 pos = transform.position;
        pos.x = Mathf.MoveTowards(pos.x, frisbee.transform.position.x, runSpeed * Time.deltaTime);
        transform.position = pos;   // kun vandret, hun bliver i samme højde
    }

    void Face(bool right)
    {
        // Tegningen kigger mod venstre, så hun skal spejles, når hun løber mod højre
        if (sr != null) sr.flipX = spriteFacesLeft ? right : !right;
    }

    void SetWalking(bool walking)
    {
        if (animator != null) animator.SetBool("isWalking", walking);
    }
}
