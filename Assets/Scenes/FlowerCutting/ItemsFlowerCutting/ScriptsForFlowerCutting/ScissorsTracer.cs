using System.Collections.Generic;
using UnityEngine;

public class ScissorsTracer : MonoBehaviour
{
    public StemCutZone stemZone;
    public FlowerCutSwap flower;

    public float minPointDistance = 0.02f;
    public float requiredTraceLength = 0.6f;

    readonly List<Vector3> points = new();
    float tracedLength;
    bool cutDone;

    void Update()
    {
        if (cutDone) return;

        // hold mouse to “trace”
        if (!Input.GetMouseButton(0)) return;

        // only count tracing when inside stem zone
        if (stemZone == null || !stemZone.TipInside) return;

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
            flower.Cut();
        }
    }
}
