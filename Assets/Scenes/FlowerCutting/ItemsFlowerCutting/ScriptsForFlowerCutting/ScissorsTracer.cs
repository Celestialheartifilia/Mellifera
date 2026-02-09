using System.Collections.Generic;
using UnityEngine;

public class ScissorsTracer : MonoBehaviour
{
    public float minPointDistance = 0.02f;
    public float requiredTraceLength = 0.6f;

    readonly List<Vector3> points = new();
    float tracedLength;
    bool cutDone;

    StemCutZone activeStemZone;
    FlowerCutSwap activeFlower;

    void Update()
    {
        if (cutDone) return;

        // Ensure we are bound to the currently active hybrid flower
        if (activeFlower == null || activeStemZone == null)
            BindToActiveHybridFlower();

        if (activeFlower == null || activeStemZone == null)
            return;

        // Hold mouse to trace
        if (!Input.GetMouseButton(0)) return;

        // Only trace when scissors tip is inside stem zone
        if (!activeStemZone.TipInside) return;

        Vector3 p = transform.position;

        if (points.Count == 0)
        {
            points.Add(p);
            return;
        }

        float d = Vector3.Distance(points[^1], p);
        if (d < minPointDistance) return;

        tracedLength += d;
        points.Add(p);

        if (tracedLength >= requiredTraceLength)
        {
            cutDone = true;
            activeFlower.Cut();
        }
    }

    void BindToActiveHybridFlower()
    {
        // Find all hybrid flower roots
        HybridFlowerTag[] hybrids = FindObjectsOfType<HybridFlowerTag>(true);

        foreach (var hybrid in hybrids)
        {
            if (!hybrid.gameObject.activeInHierarchy)
                continue;

            // Get components from children
            activeFlower = hybrid.GetComponentInChildren<FlowerCutSwap>(true);
            activeStemZone = hybrid.GetComponentInChildren<StemCutZone>(true);

            if (activeFlower != null && activeStemZone != null)
            {
                ResetTracingState();
                return;
            }
        }

        // If we get here, no active hybrid is ready
        activeFlower = null;
        activeStemZone = null;
    }

    void ResetTracingState()
    {
        points.Clear();
        tracedLength = 0f;
        cutDone = false;
    }
}
