using UnityEngine;

public class KeepTextFacing : MonoBehaviour
{
    private Vector3 startScale;

    private void Awake()
    {
        startScale = transform.localScale;
    }

    private void LateUpdate()
    {
        Vector3 scale = startScale;

        if (transform.parent.lossyScale.x < 0)
            scale.x = -scale.x;

        transform.localScale = scale;
    }
}