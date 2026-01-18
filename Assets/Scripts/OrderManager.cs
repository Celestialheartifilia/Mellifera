using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [Header("Current customer order")]
    public OrderData currentOrder;

    // Convenience property so other scripts don’t touch OrderData directly
    public RecipeData CurrentRecipe
    {
        get
        {
            if (currentOrder == null)
                return null;

            return currentOrder.requiredRecipe;
        }
    }

    // Call this when a new customer arrives
    public void SetOrder(OrderData newOrder)
    {
        currentOrder = newOrder;
    }

    // Call this when order is completed / customer leaves
    public void ClearOrder()
    {
        currentOrder = null;
    }
}
