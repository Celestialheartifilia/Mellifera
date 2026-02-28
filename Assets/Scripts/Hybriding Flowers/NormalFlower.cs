using UnityEngine;

public class NormalFlower : MonoBehaviour
{
    public ItemsSOScript flowerData;
    public bool isPollinated;

    [Header("Speech Bubble")]
    public GameObject speechIndicator;   // single speech object

    void Awake()
    {
        if (speechIndicator != null)
            speechIndicator.SetActive(false);
    }

    public void SetPollinated(bool value)
    {
        isPollinated = value;
        // Later: change sprite / play effect here

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BeeController>() != null)
        {
            if (speechIndicator != null)
                speechIndicator.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<BeeController>() != null)
        {
            if (speechIndicator != null)
                speechIndicator.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        BeeController bee = FindObjectOfType<BeeController>();
        bee.MoveToFlower(this);
    }

}
