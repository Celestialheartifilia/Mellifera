using UnityEngine;

//this script is just meant for hybrid flower identity/data tag


public class HybridFlowerTag : MonoBehaviour
{
    [Header("Speech Bubble")]
    public GameObject speechIndicator;

    public ItemsSOScript flowerItemData;// single speech object
    void Awake()
    {
        if (speechIndicator != null)
            speechIndicator.SetActive(false);
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
}
