using UnityEngine;

public class Pot : MonoBehaviour
{
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

        if (hybridFlowerManager != null)
        {
            hybridFlowerManager.OnHybridReadyToCut(plantedHybrid);
        }
    }

    public bool IsReadyToFertilise()
    {
        return growthState == FlowerGrowthState.Planted;
    }
}
