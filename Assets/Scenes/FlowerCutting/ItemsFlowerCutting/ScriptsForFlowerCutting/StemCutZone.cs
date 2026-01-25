using UnityEngine;

public class StemCutZone : MonoBehaviour
{
    public bool TipInside { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<ScissorsTracer>() != null)
            TipInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponentInParent<ScissorsTracer>() != null)
            TipInside = false;
    }
}
