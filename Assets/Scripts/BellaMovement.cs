using UnityEngine;

public class BellaMovement : MonoBehaviour
{
    // ---------- INDSTILLINGER (kan ændres i Inspector for hvert level) ----------

    [SerializeField] private Transform[] positions;     // punkterne hun hopper hen til, i rækkefølge (træk tomme GameObjects herind)
    [SerializeField] private float hopDuration = 0.8f;  // hvor mange sekunder ét hop tager
    [SerializeField] private float hopHeight = 2f;      // hvor højt buen går over den lige linje
    [SerializeField] private float waitTime = 1f;       // hvor længe hun venter på hvert punkt før næste hop

    // ---------- INTERNE VARIABLER (styres af scriptet selv) ----------

    private Rigidbody2D rb;
    private int currentStopPosition = 0;  // hvilket punkt i listen hun er på vej hen til
    private bool hasStarted = false;      // er Tom kommet ind i hitboxen endnu?
    private bool isWaiting = false;       // står hun og venter på et punkt lige nu?

    private Vector2 startPos;             // hvor hun stod, da det nuværende hop begyndte
    private float hopTimer = 0f;          // hvor mange sekunder hun har været i gang med det nuværende hop

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // HUSK: Rigidbody2D skal stå til "Kinematic" i Inspector,
        // ellers trækker tyngdekraften i hende, mens vi flytter hende.
    }

    // Kører automatisk, når noget går ind i Bellas trigger-collider (hendes hitbox)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasStarted) return;               // er hun allerede i gang, så ignorer

        if (other.CompareTag("Player"))       // er det Tom?
        {
            hasStarted = true;
            startPos = rb.position;           // første hop starter der, hvor hun står nu
            hopTimer = 0f;
        }
    }

    void FixedUpdate()
    {
        // Tre grunde til IKKE at bevæge sig:
        if (!hasStarted) return;                                // Tom er ikke kommet endnu
        if (isWaiting) return;                                  // hun holder pause på et punkt
        if (currentStopPosition >= positions.Length) return;    // hun har været ved alle punkter, ruten er færdig

        // 1. Tæl tiden op for det nuværende hop
        hopTimer += Time.fixedDeltaTime;

        // 2. Regn ud hvor langt hun er i hoppet, som et tal fra 0 til 1
        //    0 = lige startet, 0.5 = midtvejs, 1 = landet
        //    Clamp01 sørger for, at tallet aldrig bliver over 1
        float t = Mathf.Clamp01(hopTimer / hopDuration);

        // 3. Hvor skal hun hen?
        Vector2 target = positions[currentStopPosition].position;

        // 4. Den lige linje fra start til mål
        Vector2 newPos = Vector2.Lerp(startPos, target, t);

        // 5. Læg buen oveni y
        //    Sin(t * PI) er 0 ved start, 1 midtvejs og 0 ved landing, altså en bue
        newPos.y += Mathf.Sin(t * Mathf.PI) * hopHeight;

        // 6. Flyt Bella derhen (MovePosition er den rigtige måde at flytte en Kinematic Rigidbody på)
        rb.MovePosition(newPos);

        // 7. Er hun landet?
        if (t >= 1f)
        {
            isWaiting = true;                                // så denne del ikke kører igen næste frame
            Invoke(nameof(MoveToNextPosition), waitTime);    // start ÉN timer, så hun hopper videre efter pausen
        }

        
    }

    // Kaldes af Invoke, når pausen er slut
    private void MoveToNextPosition()
    {
        currentStopPosition++;
        if (currentStopPosition >= positions.Length)   // 5 >= 5, altså ingen flere punkter
        {
            Destroy(gameObject);
            return;
        }
            startPos = rb.position;      // næste hop starter der, hvor hun landede
        hopTimer = 0f;               // nulstil tiden til det nye hop
        isWaiting = false;           // nu må hun bevæge sig igen
    }

    // Tegner ruten i Scene-vinduet, så det er nemt at placere punkterne (kun synligt i editoren)
    void OnDrawGizmos()
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