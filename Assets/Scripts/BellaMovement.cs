using UnityEngine;

public class BellaMovement : MonoBehaviour
{ 
    [SerializeField] private Transform[] positions;     // punkterne hun hopper hen til, i rækkefølge (træk tomme GameObjects herind)
    [SerializeField] private float hopDuration = 0.8f;  
    [SerializeField] private float hopHeight = 2f;      
    [SerializeField] private float waitTime = 1f;       
    private Rigidbody2D rb;
    private int currentStopPosition = 0;  
    private bool hasStarted = false;      // om Tom kommet ind i hitboxen endnu
    private bool isWaiting = false;       // står hun og venter på et punkt lige nu?

    private Vector2 startPos;            
    private float hopTimer = 0f;         

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))       
        {
            hasStarted = true;
            startPos = rb.position;           
            hopTimer = 0f;
        }
    }
    void FixedUpdate()
    {
        if (!hasStarted) return;                                // Tom er ikke kommet endnu
        if (isWaiting) return;                                  // hun holder pause på et punkt
        if (currentStopPosition >= positions.Length) return;    // hun har været ved alle punkter, ruten er færdig

        hopTimer += Time.fixedDeltaTime;
        float t = Mathf.Clamp01(hopTimer / hopDuration);

        Vector2 target = positions[currentStopPosition].position;

        Vector2 newPos = Vector2.Lerp(startPos, target, t);

       
        //    Sin(t * PI) er 0 ved start, 1 midtvejs og 0 ved landing, altså en bue
        newPos.y += Mathf.Sin(t * Mathf.PI) * hopHeight;
      
        rb.MovePosition(newPos);
 
        if (t >= 1f)
        {
            isWaiting = true;                                
            Invoke(nameof(MoveToNextPosition), waitTime);    
        }
    } 
    private void MoveToNextPosition()
    {
        currentStopPosition++;
        if (currentStopPosition >= positions.Length)   
        {
            Destroy(gameObject);
            return;
        }
        startPos = rb.position;      
        hopTimer = 0f;               
        isWaiting = false;           
    }

 
    void OnDrawGizmos() //tegner streger så det gør det nemmere at se hvor linjen for bevægelsen af bella
    {
        if (positions == null) return;

        Gizmos.color = Color.yellow;
        Vector3 previous = transform.position;
        foreach (Transform p in positions)
        {
            if (p == null) continue;
            Gizmos.DrawLine(previous, p.position);
            Gizmos.DrawWireSphere(p.position, 0.2f);
            previous = p.position;
        }
    }
}