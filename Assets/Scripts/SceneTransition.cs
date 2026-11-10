using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad = "Level 1 test";
    public IrisTransition iris;     // træk IrisHul-objektet herind (findes automatisk hvis tomt)

    bool started = false;           // så overgangen ikke kan starte to gange

    void Start()
    {
        if (iris == null) iris = FindFirstObjectByType<IrisTransition>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Bella rammer scene-skiftet: hun forsvinder, og tager en kastet frisbee med sig
        BellaFetch bella = collision.GetComponentInParent<BellaFetch>();
        if (bella != null)
        {
            Frisbee looseFrisbee = FindFirstObjectByType<Frisbee>();
            if (looseFrisbee != null && looseFrisbee.transform.parent == null)
                Destroy(looseFrisbee.gameObject);
            Destroy(bella.gameObject);
            return;
        }
        // En kastet frisbee (som ingen holder) bliver liggende her, så Bella kan nå hen til den
        if (collision.GetComponent<Frisbee>() != null && collision.transform.parent == null)
        {
            Rigidbody2D frisbeeRb = collision.attachedRigidbody;
            if (frisbeeRb != null)
            {
                frisbeeRb.linearVelocity = Vector2.zero;
                frisbeeRb.angularVelocity = 0f;
                frisbeeRb.bodyType = RigidbodyType2D.Kinematic;
            }
            return;
        }

        if (started) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            started = true;
            LockPlayer(collision.gameObject);
            StartCoroutine(Transition());
        }
    }

    // 2. Spilleren mister kontrollen
    void LockPlayer(GameObject player)
    {
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = false;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // stop med at glide

        Animator anim = player.GetComponent<Animator>();
        if (anim != null) anim.SetBool("isWalking", false);
    }

    IEnumerator Transition()
    {
        // 3. Cirklen lukker sig
        if (iris != null)
            yield return StartCoroutine(iris.Close());

        // 4. Først derefter skifter scenen
        SceneManager.LoadScene(sceneToLoad);
    }
}
