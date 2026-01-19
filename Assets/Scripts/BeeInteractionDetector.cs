using UnityEngine;

public class BeeInteractionDetector : MonoBehaviour
{
    public Flower currentFlower;
    public bool nearPot;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Flower"))
        {
            currentFlower = other.GetComponent<Flower>();
            Debug.Log("Entered Flower trigger: " + other.name + " | Has Flower component? " + (currentFlower != null));
        }

        if (other.CompareTag("Pot"))
        {
            nearPot = true;
            Debug.Log("Entered Pot trigger: " + other.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Flower"))
        {
            Flower f = other.GetComponent<Flower>();
            if (currentFlower == f)
                currentFlower = null;

            Debug.Log("Exited Flower trigger: " + other.name);
        }

        if (other.CompareTag("Pot"))
        {
            nearPot = false;
            Debug.Log("Exited Pot trigger: " + other.name);
        }
    }
}
