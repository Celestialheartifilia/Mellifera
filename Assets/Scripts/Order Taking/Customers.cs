using UnityEngine;

public class Customers : MonoBehaviour
{
    public GameObject customer1;
    public GameObject customer2;
    public GameObject customer3;

    public OrderTakingManager orderTakingManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (customer1 != null)
        {
            customer1.SetActive(true);
            Debug.Log("customer 1 order is small");
            orderTakingManager.CreateNewOrder(OrderTakingManager.OrderType.Small);
            Debug.Log(OrderTakingManager.OrderType.Small);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
