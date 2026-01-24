using System.Collections.Generic;
using UnityEngine;

public class HybridFlowerSceneManager : MonoBehaviour
{
    private OrderTakingManager orderTakingManager;

    public List<ItemsSOScript> requiredHybrids = new List<ItemsSOScript>();

    void Start()
    {
        orderTakingManager = OrderTakingManager.Instance;

        requiredHybrids.Clear();

        if (orderTakingManager == null || orderTakingManager.currentOrder == null)
        {
            Debug.LogWarning("No order found. Scene has nothing to do.");
            return;
        }

        foreach (var item in orderTakingManager.currentOrder.orderedItems)
        {
            if (orderTakingManager.hybridFlowerItems.Contains(item))
            {
                requiredHybrids.Add(item);
            }
        }

        Debug.Log("Required hybrids: " + requiredHybrids.Count);
    }

    public void OnHybridHarvested(ItemsSOScript harvestedHybrid)
    {
        if (requiredHybrids.Contains(harvestedHybrid))
        {
            requiredHybrids.Remove(harvestedHybrid);

            if (requiredHybrids.Count == 0)
            {
                Debug.Log("All hybrids done!");
                // next scene / return to shop later
            }
        }
    }
}
