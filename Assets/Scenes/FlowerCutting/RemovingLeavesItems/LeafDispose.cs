using UnityEngine;

public class LeafDispose : MonoBehaviour
{
    [Header("Reference")]
    public Collider2D binCollider;

    [Header("Behaviour")]
    public bool destroyInsteadOfDisable = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != binCollider) return;

        if (destroyInsteadOfDisable)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}
