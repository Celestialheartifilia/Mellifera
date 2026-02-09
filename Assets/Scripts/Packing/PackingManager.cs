using UnityEngine;
using UnityEngine.UI;

public class PackingManager : MonoBehaviour
{
    [Header("Hybrid Flowers (Gameplay)")]
    public GameObject hybridFlower1;
    public GameObject hybridFlower2;

    [Header("Hybrid Buttons (UI)")]
    public Button hybridButton1;
    public Button hybridButton2;

    ItemsSOScript collectedHybrid;

    void Start()
    {
        // Safety
        if (GameState.Instance == null || GameState.Instance.collectedHybrids.Count == 0)
        {
            Debug.LogError("No collected hybrid data found.");
            return;
        }

        collectedHybrid = GameState.Instance.collectedHybrids[0];

        // Hide everything first
        hybridFlower1.SetActive(false);
        hybridFlower2.SetActive(false);

        hybridButton1.gameObject.SetActive(false);
        hybridButton2.gameObject.SetActive(false);

        // Get data tags
        var tag1 = hybridFlower1.GetComponent<HybridFlowerTag>();
        var tag2 = hybridFlower2.GetComponent<HybridFlowerTag>();

        if (tag1 == null || tag2 == null)
        {
            Debug.LogError("HybridFlowerTag missing on flower GameObjects.");
            return;
        }

        // Decide which button to show
        if (tag1.flowerItemData == collectedHybrid)
        {
            hybridButton1.gameObject.SetActive(true);
            hybridButton1.onClick.AddListener(() => ActivateHybridFlower(hybridFlower1));
        }
        else if (tag2.flowerItemData == collectedHybrid)
        {
            hybridButton2.gameObject.SetActive(true);
            hybridButton2.onClick.AddListener(() => ActivateHybridFlower(hybridFlower2));
        }
        else
        {
            Debug.LogError("Collected hybrid does not match any flower.");
        }
    }

    void ActivateHybridFlower(GameObject flower)
    {
        // Hide buttons after selection
        hybridButton1.gameObject.SetActive(false);
        hybridButton2.gameObject.SetActive(false);

        // Activate gameplay object
        flower.SetActive(true);
    }

}
