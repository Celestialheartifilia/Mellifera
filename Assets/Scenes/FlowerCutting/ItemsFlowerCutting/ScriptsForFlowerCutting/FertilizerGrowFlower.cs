using System.Collections;
using UnityEngine;

public class FertilizerGrowFlower : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject flowerObject;   // Flower to appear
    public Collider2D potCollider;    // Pot collider

    Vector3 originalPosition;
    bool triggered = false;

    void Start()
    {
        // Save fertilizer starting position
        originalPosition = transform.position;

        // Hide flower at start
        if (flowerObject != null)
            flowerObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        // Detect pot WITHOUT using tag
        if (other == potCollider)
        {
            triggered = true;
            StartCoroutine(GrowAndReturn());
        }
    }

    IEnumerator GrowAndReturn()
    {
        yield return new WaitForSeconds(2f);

        // Show flower
        if (flowerObject != null)
            flowerObject.SetActive(true);

        // Return fertilizer to original position
        transform.position = originalPosition;
    }
}
