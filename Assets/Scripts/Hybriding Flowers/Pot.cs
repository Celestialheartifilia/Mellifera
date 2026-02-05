using UnityEngine;

public class Pot : MonoBehaviour
{
    [Header("Tool Colliders")]
    public Collider2D fertiliserCollider;
    public Collider2D scissorsCollider;
    public GameObject BeeGone;

    [Header("UI (Optional)")]
    public GameObject toolsReadyPopup;

    void Start()
    {
        if (fertiliserCollider != null)
            fertiliserCollider.enabled = false;

        if (scissorsCollider != null)
            scissorsCollider.enabled = false;

        if (BeeGone != null)
            BeeGone.SetActive(true);

        if (toolsReadyPopup != null)
            toolsReadyPopup.SetActive(false);
    }


    //public bool isEmpty = true;
    public enum FlowerGrowthState
    {
        Empty,
        Planted,
        Fertilised,
        Grown
    }

    public FlowerGrowthState growthState = FlowerGrowthState.Empty;

    public HybridFlowerManager hybridFlowerManager;

    public ItemsSOScript plantedHybrid;

    public void Plant(ItemsSOScript hybrid)
    {
        //isEmpty = false;
        if (growthState != FlowerGrowthState.Empty)
        {
            return;
        }

        plantedHybrid = hybrid;
        growthState = FlowerGrowthState.Planted;

        Debug.Log("Hybrid planted: " + hybrid.itemName);
    }

    public void Fertilise()
    {
        if (growthState != FlowerGrowthState.Planted)
        {
            return;
        }

        growthState = FlowerGrowthState.Fertilised;
        Debug.Log("Fertilizer applied");

        Grow();
    }

    public void Grow()
    {
        if (growthState != FlowerGrowthState.Fertilised)
            return;

        growthState = FlowerGrowthState.Grown;
        Debug.Log("Flower fully grown");

        // Enable fertiliser + scissors colliders
        if (fertiliserCollider != null)
            fertiliserCollider.enabled = true;

        if (scissorsCollider != null)
            scissorsCollider.enabled = true;

        if (BeeGone != null)
            BeeGone.SetActive(false);


        // Optional popup
        if (toolsReadyPopup != null)
            toolsReadyPopup.SetActive(true);

        if (hybridFlowerManager != null)
        {
            hybridFlowerManager.OnHybridReadyToCut(plantedHybrid);
        }
    }

    // Add Code so that when done the fertilizer and scissors can use maybe a ui Pop up 
    public bool IsReadyToFertilise()
    {
        return growthState == FlowerGrowthState.Planted;
    }
}
