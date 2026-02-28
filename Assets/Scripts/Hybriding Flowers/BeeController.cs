using System.Collections;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("References")]
    public PollinationManager pollinationManager;

    [Header("Direction Animation")]
    public GameObject frontObj;
    public GameObject backObj;
    public GameObject leftObj;
    public GameObject rightObj;

    [Header("Disappear Animation")]
    public float disappearDuration = 0.25f;

    Rigidbody2D rb;

    Vector3 targetPosition;
    bool hasTarget = false;

    Vector3 startPosition;
    Vector3 originalScale;

    NormalFlower currentFlower;
    Pot currentPot;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        startPosition = transform.position;
        originalScale = transform.localScale;

        ShowOnly(frontObj);
    }

    void Update()
    {
        if (hasTarget)
        {
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        Vector2 dir = (targetPosition - transform.position);

        if (dir.magnitude < 0.05f)
        {
            rb.linearVelocity = Vector2.zero;
            hasTarget = false;

            OnReachedTarget();
            return;
        }

        dir.Normalize();
        rb.linearVelocity = dir * moveSpeed;

        // Direction animation
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            ShowOnly(dir.x > 0 ? rightObj : leftObj);
        else
            ShowOnly(dir.y > 0 ? backObj : frontObj);
    }

    void OnReachedTarget()
    {
        // Pollinate flower
        if (currentFlower != null && !currentFlower.isPollinated)
        {
            pollinationManager.TryAddPollinatedFlower(currentFlower);
            currentFlower = null;
            return;
        }

        // Plant into pot
        if (currentPot != null && pollinationManager.PollinationCount == 2)
        {
            bool planted = pollinationManager.TryPlantInto(currentPot);

            if (planted)
            {
                ReturnToStart();
            }

            currentPot = null;
        }
    }

    // ===============================
    // Public Movement Commands
    // ===============================

    public void MoveToFlower(NormalFlower flower)
    {
        if (flower.isPollinated)
            return;

        currentFlower = flower;
        currentPot = null;

        targetPosition = flower.transform.position;
        hasTarget = true;
    }

    public void MoveToPot(Pot pot)
    {
        if (pollinationManager.PollinationCount < 2)
            return;

        currentPot = pot;
        currentFlower = null;

        targetPosition = pot.transform.position;
        hasTarget = true;
    }

    public void ReturnToStart()
    {
        currentFlower = null;
        currentPot = null;

        targetPosition = startPosition;
        hasTarget = true;
    }

    // ===============================
    // Animations
    // ===============================

    void ShowOnly(GameObject obj)
    {
        if (frontObj) frontObj.SetActive(obj == frontObj);
        if (backObj) backObj.SetActive(obj == backObj);
        if (leftObj) leftObj.SetActive(obj == leftObj);
        if (rightObj) rightObj.SetActive(obj == rightObj);
    }

}


