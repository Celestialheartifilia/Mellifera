using UnityEngine;

public class BeeInteractionDetector : MonoBehaviour
{
    public Flower currentFlower; // The Flower script we are touching (null if none)
    public bool nearPot;         // True if touching pot trigger

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Flower"))
        {
            currentFlower = other.GetComponent<Flower>();
        }

        if (other.CompareTag("Pot"))
        {
            nearPot = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Flower"))
        {
            Flower f = other.GetComponent<Flower>();
            if (currentFlower == f)
                currentFlower = null;
        }

        if (other.CompareTag("Pot"))
        {
            nearPot = false;
        }
    }
}
