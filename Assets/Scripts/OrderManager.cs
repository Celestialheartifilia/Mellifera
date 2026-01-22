using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderManager : MonoBehaviour
{
    // Which customer are we generating an order for?
    public enum CustomerType 
    { 
        Customer1,
        Customer2,
        Customer3
    }

    [Header("Random pools (drag options here)")]
    public FlowerType[] normalFlowerPool; // Flower1/2/3 only
    public Sprite[] wrapPool;
    public Sprite[] accessoryPool;

    [Header("Current order (runtime)")]
    public CustomerOrder currentOrder;

    [Header("Testing")]
    public bool autoStart = true;

    void Start()
    {
        if (autoStart) 
        {
            StartCustomer(CustomerType.Customer1);
        }
        
    }

    // MAIN: Start a customer order
    public void StartCustomer(CustomerType type)
    {
        currentOrder = new CustomerOrder();     // create a fresh order
        AssignRandomPacking();                  // wrap + accessory always random
        AssignItemsWanted(type);                // what flowers they want (your rules)
        ResetProgress();                        // set progress back to 0

        Debug.Log("[OrderManager] New order: " + type);
    }

    // ORDER CONTENT (what they want)
    void AssignItemsWanted(CustomerType type)
    {
        if (type == CustomerType.Customer1)
        {
            // Customer 1: Hybrid1 only
            currentOrder.requiredHybrids = new FlowerType[] { FlowerType.Hybrid1 };
            currentOrder.normalRequired = false;
        }
        else if (type == CustomerType.Customer2)
        {
            // Customer 2: Hybrid2 + 1 random normal flower
            currentOrder.requiredHybrids = new FlowerType[] { FlowerType.Hybrid2 };
            currentOrder.normalRequired = true;
            currentOrder.requiredNormal = RandomNormalFlower();
        }
        else
        {
            // Customer 3: Hybrid1 + Hybrid2
            currentOrder.requiredHybrids = new FlowerType[] { FlowerType.Hybrid1, FlowerType.Hybrid2 };
            currentOrder.normalRequired = false;
        }
    }

    void AssignRandomPacking()
    {
        currentOrder.wrapIcon = RandomSprite(wrapPool);
        currentOrder.accessoryIcon = RandomSprite(accessoryPool);
    }

    void ResetProgress()
    {
        currentOrder.hybridsMade = 0;
        //currentOrder.normalPrepared = false;
    }

    // ASK: What hybrid should be made RIGHT NOW?
    public bool TryGetNextRequiredHybrid(out FlowerType nextHybrid)
    {
        nextHybrid = FlowerType.Hybrid1; // default value if we return false

        if (currentOrder == null) return false;
        if (currentOrder.requiredHybrids == null) return false;

        int nextIndex = currentOrder.hybridsMade;

        // If we already made all required hybrids, there is no next one
        if (nextIndex >= currentOrder.requiredHybrids.Length) return false;

        nextHybrid = currentOrder.requiredHybrids[nextIndex];
        return true;
    }

    // ----------------------------
    // PROGRESS: Mark things done
    // ----------------------------
    public void MarkHybridDone()
    {
        if (currentOrder == null) return;

        currentOrder.hybridsMade++;
        Debug.Log("[OrderManager] Hybrids made: " + currentOrder.hybridsMade + "/" + currentOrder.requiredHybrids.Length);
    }

    public void MarkNormalDone()
    {
        if (currentOrder == null) return;

        currentOrder.normalDone = true;
        Debug.Log("[OrderManager] Normal flower done");
    }

    public bool IsOrderComplete()
    {
        if (currentOrder == null) return false;
        return currentOrder.IsComplete();
    }

    // RANDOM HELPERS
    FlowerType RandomNormalFlower()
    {
        if (normalFlowerPool == null || normalFlowerPool.Length == 0)
            return FlowerType.Flower1;

        return normalFlowerPool[Random.Range(0, normalFlowerPool.Length)];
    }

    Sprite RandomSprite(Sprite[] pool)
    {
        if (pool == null || pool.Length == 0)
            return null;

        return pool[Random.Range(0, pool.Length)];
    }
}
