using UnityEngine;

public class BeeInteractionDetector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Flower currentFlower; // the flower the bee is touching (null if none)
    public bool nearPot;         // true if the bee is touching the pot trigger

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If bee enters a flower trigger, remember that flower
        if (other.CompareTag("Flower"))
        {
            currentFlower = other.GetComponent<Flower>();
            Debug.Log("Collide with Flower");
        }

        // If bee enters pot trigger, set nearPot
        if (other.CompareTag("Pot"))
        {
            nearPot = true;
            Debug.Log("Collide with Pot");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If bee leaves the flower it was touching, clear it
        if (other.CompareTag("Flower"))
        {
            var f = other.GetComponent<Flower>();
            if (currentFlower == f) currentFlower = null;
        }

        if (other.CompareTag("Pot"))
        {
            nearPot = false;
        }
    }
}
