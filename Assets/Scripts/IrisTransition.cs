using System.Collections;
using UnityEngine;

// Sidder på IrisHul-objektet.
// Idé: en kæmpe sort firkant dækker ALT, og en cirkel-formet SpriteMask
// skærer et hul i den. Kun hullet krymper - det sorte bliver liggende,
// så man aldrig kan se banen uden om. Når hullet er 0, er skærmen helt sort.
public class IrisTransition : MonoBehaviour
{
    [Tooltip("Hvor mange sekunder det tager at lukke cirklen")]
    public float closeDuration = 1.2f;

    [Tooltip("Hvor længe skærmen forbliver helt sort, før scenen skifter")]
    public float holdBlack = 0.3f;

    GameObject sort;      // den sorte firkant
    Transform hul;        // masken der laver hullet

    void Awake()
    {
        // Det gamle billede bruges ikke længere - kun objektets position (= hullets midte)
        SpriteRenderer gammel = GetComponent<SpriteRenderer>();
        int order = 50;
        if (gammel != null) { order = gammel.sortingOrder; gammel.enabled = false; }
        transform.localScale = Vector3.one;

        // Sort firkant, der er større end hele banen
        sort = new GameObject("IrisSort");
        sort.transform.SetParent(transform, false);
        sort.transform.localScale = Vector3.one * 5000f;
        SpriteRenderer sr = sort.AddComponent<SpriteRenderer>();
        sr.sprite = LavFirkant();
        sr.color = Color.black;
        sr.sortingOrder = order;
        sr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask; // vis kun UDEN FOR hullet

        // Hullet
        GameObject hulObj = new GameObject("IrisHulMaske");
        hulObj.transform.SetParent(transform, false);
        SpriteMask mask = hulObj.AddComponent<SpriteMask>();
        mask.sprite = LavCirkel();
        hul = hulObj.transform;

        sort.SetActive(false);   // skjult indtil overgangen starter
    }

    // Lukker cirklen. Kaldes med: yield return StartCoroutine(iris.Close());
    public IEnumerator Close()
    {
        // Start med et hul der præcis dækker hele skærmen
        float start = BeregnStartDiameter();
        hul.localScale = Vector3.one * start;
        sort.SetActive(true);

        float tid = 0f;
        while (tid < closeDuration)
        {
            tid += Time.deltaTime;
            float t = Mathf.Clamp01(tid / closeDuration);   // 0 -> 1
            t = t * t * (3f - 2f * t);                      // blød start og blød slutning
            hul.localScale = Vector3.one * Mathf.Lerp(start, 0f, t);
            yield return null;                              // vent til næste frame
        }
        hul.localScale = Vector3.zero;                      // helt sort

        yield return new WaitForSeconds(holdBlack);
    }

    // Afstanden fra hullets midte til det fjerneste hjørne af skærmen, gange 2
    float BeregnStartDiameter()
    {
        Camera cam = Camera.main;
        if (cam == null) return 100f;
        float h = cam.orthographicSize;
        float w = h * cam.aspect;
        Vector2 c = cam.transform.position;
        Vector2 p = transform.position;
        float max = 0f;
        foreach (Vector2 hjorne in new[] { c + new Vector2(w, h), c + new Vector2(-w, h), c + new Vector2(w, -h), c + new Vector2(-w, -h) })
            max = Mathf.Max(max, Vector2.Distance(p, hjorne));
        return max * 2f * 1.05f;
    }

    // Laver en 1x1 enhed hvid firkant i kode
    static Sprite LavFirkant()
    {
        Texture2D tex = new Texture2D(4, 4);
        Color[] px = new Color[16];
        for (int i = 0; i < px.Length; i++) px[i] = Color.white;
        tex.SetPixels(px); tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f), 4f);
    }

    // Laver en cirkel med diameter 1 enhed i kode
    static Sprite LavCirkel()
    {
        int s = 256;
        Texture2D tex = new Texture2D(s, s);
        Color[] px = new Color[s * s];
        float r = s / 2f;
        for (int y = 0; y < s; y++)
            for (int x = 0; x < s; x++)
            {
                float d = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(r, r));
                px[y * s + x] = d <= r ? Color.white : Color.clear;
            }
        tex.SetPixels(px); tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, s, s), new Vector2(0.5f, 0.5f), s);
    }
}
