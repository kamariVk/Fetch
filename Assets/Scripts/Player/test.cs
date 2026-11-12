using Unity.VisualScripting;
using UnityEngine;

public class grappleTest : MonoBehaviour
{
    [SerializeField] private GameObject closedHand;
    [SerializeField] private GameObject openHand;
    [SerializeField]private Grapple grapple;

  

    public void Update()
    {
        if (grapple.isGrappling)
        {
            closedHand.SetActive(true);
            openHand.SetActive(false);
        }
        else
        {
            closedHand.SetActive(false);
            openHand.SetActive(true);
        }
    }
}

