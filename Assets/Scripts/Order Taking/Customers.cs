using UnityEngine;
using UnityEngine.SceneManagement;

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
            OrderTakingManager.Instance.CreateNewOrder(OrderTakingManager.OrderType.Small);
            Debug.Log(OrderTakingManager.OrderType.Small);
            SceneManager.LoadScene("HybridingFlowerScene");
        }

        //if (customer2 != null)
        //{
        //    customer2.SetActive(true);
        //    Debug.Log("customer 2 order is medium");
        //    orderTakingManager.CreateNewOrder(OrderTakingManager.OrderType.Medium);
        //    Debug.Log(OrderTakingManager.OrderType.Medium);
        //}

        //if (customer3 != null)
        //{
        //    customer3.SetActive(true);
        //    Debug.Log("customer 3 order is big");
        //    orderTakingManager.CreateNewOrder(OrderTakingManager.OrderType.Big);
        //    Debug.Log(OrderTakingManager.OrderType.Big);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
