using System.Collections;
using UnityEngine;

public class FertilizerGrowFlower : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Pot pot;                
    public Collider2D potCollider;

    Vector3 originalPosition;
    bool triggered = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other != potCollider) return;

        triggered = true;

        bool success = pot.Fertilise();

        if (success)
        {
            StartCoroutine(ReturnFertiliser());
        }
        else
        {
            triggered = false; // allow retry if fertilising failed
        }
    }

    IEnumerator ReturnFertiliser()
    {
        yield return new WaitForSeconds(1f);
        transform.position = originalPosition;
        triggered = false;
    }
}
