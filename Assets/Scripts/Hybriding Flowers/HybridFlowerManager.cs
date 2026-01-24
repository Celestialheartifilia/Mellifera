using System.Collections.Generic;
using UnityEngine;

public class HybridFlowerManager : MonoBehaviour
{
    private OrderTakingManager orderTakingManager;

    public List<ItemsSOScript> remainingRequiredHybrids = new();

    void Start()
    {
        orderTakingManager = OrderTakingManager.Instance;

        remainingRequiredHybrids.Clear();

        if (orderTakingManager == null || orderTakingManager.currentOrder == null)
        {
            Debug.LogWarning("No order found. Scene has nothing to do.");
            return;
        }

        foreach (var item in orderTakingManager.currentOrder.orderedItems)
        {
            if (orderTakingManager.hybridFlowerItems.Contains(item))
            {
                remainingRequiredHybrids.Add(item);
            }
        }

        Debug.Log("Hybrids required: " + remainingRequiredHybrids.Count);
    }

    public void OnHybridHarvested(ItemsSOScript harvestedHybrid)
    {
        if (!remainingRequiredHybrids.Contains(harvestedHybrid))
            return;

        remainingRequiredHybrids.Remove(harvestedHybrid);

        if (remainingRequiredHybrids.Count == 0)
        {
            Debug.Log("All hybrids completed!");
            // enable next button / return to shop
        }
    }
}
