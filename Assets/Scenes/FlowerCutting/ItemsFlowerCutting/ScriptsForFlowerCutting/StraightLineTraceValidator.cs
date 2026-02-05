using UnityEngine;

public class StraightLineTraceValidator : MonoBehaviour
{
    [Header("Line")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Cut Zone (REQUIRED)")]
    public Collider2D cutZoneCollider;

    [Header("Scissors (for reset)")]
    public ScissorMove scissorsDrag;

    [Header("Tip (REQUIRED)")]
    public Transform tip;

    [Header("Colliders")]
    public Collider2D stemCollider;
    public Collider2D[] wrongFlowerColliders;

    [Header("UI")]
    public GameObject wrongPrompt;
    public GameObject correctPrompt;
    public GameObject traceLineObject; // assign TraceZone here (GameObject)

    [Header("Line Rules")]
    public float maxDistanceFromLine = 0.08f;
    public float minForwardProgress = 0.01f;
    public float completionThreshold = 0.95f;

    [Header("Input")]
    public bool requireMouseHold = true;
    public float minMoveToStart = 0.02f;

    [Header("On success")]
    public FlowerCutSwap flower;

    float bestProgress;
    bool done;

    Vector2 prevPos;
    bool hasPrev;
    bool startedStroke;
    bool touchedStem;
    bool armed;

    void Update()
    {
        if (done) return;
        if (startPoint == null || endPoint == null || tip == null) return;
        if (cutZoneCollider == null) return;

        if (requireMouseHold && !Input.GetMouseButton(0))
        {
            ResetAttempt();
            return;
        }

        Vector2 A = startPoint.position;
        Vector2 B = endPoint.position;
        Vector2 P = tip.position;

        if (!hasPrev)
        {
            prevPos = P;
            hasPrev = true;
            return;
        }

        // Arm only when tip touches stem once
        if (!armed)
        {
            if (stemCollider != null && stemCollider.OverlapPoint(P))
            {
                armed = true;
                touchedStem = true;
                bestProgress = 0f;
                startedStroke = false;
                prevPos = P;

                if (wrongPrompt != null) wrongPrompt.SetActive(false);
            }
            else
            {
                prevPos = P;
                if (wrongPrompt != null) wrongPrompt.SetActive(false);
                return;
            }
        }

        float moved = Vector2.Distance(prevPos, P);
        if (!startedStroke)
        {
            if (moved < minMoveToStart)
            {
                prevPos = P;
                return;
            }
            startedStroke = true;
        }

        // ❌ wrong if leave cut zone
        if (!cutZoneCollider.OverlapPoint(P))
        {
            FailAndReset();
            return;
        }

        // ❌ wrong if touch petals/leaves/head
        foreach (var wrong in wrongFlowerColliders)
        {
            if (wrong != null && wrong.OverlapPoint(P))
            {
                FailAndReset();
                return;
            }
        }

        if (stemCollider != null && stemCollider.OverlapPoint(P))
            touchedStem = true;

        prevPos = P;

        Vector2 AB = B - A;
        float abSqr = AB.sqrMagnitude;
        if (abSqr < 0.00001f) return;

        float t = Vector2.Dot(P - A, AB) / abSqr;
        t = Mathf.Clamp01(t);

        Vector2 closest = A + AB * t;
        float dist = Vector2.Distance(P, closest);

        if (dist > maxDistanceFromLine)
        {
            FailAndReset();
            return;
        }

        if (t + minForwardProgress < bestProgress)
        {
            FailAndReset();
            return;
        }

        if (t > bestProgress) bestProgress = t;

        // ✅ SUCCESS
        if (bestProgress >= completionThreshold && touchedStem)
        {
            done = true;

            if (flower != null) flower.Cut();

            // ✅ cut zone disappears after success
            if (cutZoneCollider != null)
                cutZoneCollider.gameObject.SetActive(false);

            // ✅ trace zone disappears after success
            if (traceLineObject != null)
                traceLineObject.SetActive(false);

            if (wrongPrompt != null) wrongPrompt.SetActive(false);
            if (correctPrompt != null) correctPrompt.SetActive(true);
        }
    }

    void FailAndReset()
    {
        ResetAttempt();

        if (wrongPrompt != null) wrongPrompt.SetActive(true);

        if (scissorsDrag != null)
            scissorsDrag.ResetToStart();
    }

    void ResetAttempt()
    {
        bestProgress = 0f;
        hasPrev = false;
        startedStroke = false;
        touchedStem = false;
        armed = false;

        if (wrongPrompt != null) wrongPrompt.SetActive(false);
    }
}
