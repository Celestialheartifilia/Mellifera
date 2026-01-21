using System.Collections.Generic;
using UnityEngine;
using static OrderManager;

public class OrderTakingManager : MonoBehaviour
{
    public List<ItemsSOScript> normalFlowerItems;
    public List<ItemsSOScript> hybridFlowerItems;
    public List<ItemsSOScript> wrapItems;
    public List<ItemsSOScript> accessoryItems;

    public OrderList currentOrder;

    public enum OrderType
    {
        Small,
        Medium,
        Big
    }

    // Call this when a new customer appears
    public void CreateNewOrder(OrderType orderType)
    {
        currentOrder = new OrderList();

        switch (orderType)
        {
            case OrderType.Small:
                CreateSmallOrder();
                break;

            case OrderType.Medium:
                CreateMediumOrder();
                break;

            case OrderType.Big:
                CreateBigOrder();
                break;
        }
    }

    // Call this to add an item to the order
    void CreateSmallOrder()
    {
        // 1 random hybrid flower 
        AddRandomHybridFlower();

        // 1 random wrap
        AddRandomWrap();

        // 1 random accessory
        AddRandomAccessory();
        Debug.Log("all items r ordered");
    }

    void CreateMediumOrder()
    {
        // 1 random hybrid flower
        AddRandomHybridFlower();

        // 1 random normal flower
        AddRandomNormalFlower();

        // 1 random wrap
        AddRandomWrap();

        // 1 random accessory
        AddRandomAccessory();
    }

    void CreateBigOrder()
    {
        // 2 random different hybrid flower
        ItemsSOScript randomHybridFlower1 = hybridFlowerItems[Random.Range(0, hybridFlowerItems.Count)];
        currentOrder.orderedItems.Add(randomHybridFlower1);

        ItemsSOScript randomHybridFlower2;
        do
        {
            randomHybridFlower2 = hybridFlowerItems[Random.Range(0, hybridFlowerItems.Count)];
        }
        while (randomHybridFlower2 == randomHybridFlower1);

        currentOrder.orderedItems.Add(randomHybridFlower2);

        // 1 random wrap
        AddRandomWrap();

        // 1 random accessory
        AddRandomAccessory();
    }

    #region Add random items to order methods
    void AddRandomHybridFlower()
    {
        ItemsSOScript randomHybridFlower = hybridFlowerItems[Random.Range(0, hybridFlowerItems.Count)];
        currentOrder.orderedItems.Add(randomHybridFlower);
        Debug.Log(randomHybridFlower);
    }

    void AddRandomNormalFlower()
    {
        ItemsSOScript randomNormalFlower = normalFlowerItems[Random.Range(0, normalFlowerItems.Count)];
        currentOrder.orderedItems.Add(randomNormalFlower);
        Debug.Log(randomNormalFlower);
    }

    void AddRandomWrap()
    {
        ItemsSOScript randomWrap = wrapItems[Random.Range(0, wrapItems.Count)];
        currentOrder.orderedItems.Add(randomWrap);
        Debug.Log(randomWrap);
    }

    void AddRandomAccessory()
    {
        ItemsSOScript randomAccessory = accessoryItems[Random.Range(0, accessoryItems.Count)];
        currentOrder.orderedItems.Add(randomAccessory);
        Debug.Log(randomAccessory);
    }
    #endregion

    // Call this when the order is done
    public void ClearOrder()
    {
        currentOrder = null;
    }
}
