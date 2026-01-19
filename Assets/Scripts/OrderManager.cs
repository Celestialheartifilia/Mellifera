using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public enum CustomerType
    {
        Customer1, // Hybrid 1
        Customer2, // Hybrid 2 + 1 normal
        Customer3  // Hybrid 1 + Hybrid 2
    }

    [Header("Fixed hybrid recipes (drag your RecipeData assets here)")]
    public RecipeData hybridRecipe1;
    public RecipeData hybridRecipe2;

    [Header("Random pools (drag assets here)")]
    public FlowerData[] possibleNormalFlowers;
    public Sprite[] wrapOptions;
    public Sprite[] accessoryOptions;

    [Header("Current runtime order (debug view)")]
    public CustomerOrder currentOrder;

    private void Start()
    {
        // TEST ONLY: create a customer order automatically so HybridManager can run
        if (currentOrder == null)
        {
            GenerateOrder(CustomerType.Customer1);
            Debug.Log("[OrderManager] Generated Customer 1 order (test).");
        }
    }

    // HybridManager uses this to know what recipe to validate right now
    public RecipeData CurrentRecipe
    {
        get
        {
            // No order = no recipe
            if (currentOrder == null) return null;

            // If we already made all required hybrids, no more recipe
            if (currentOrder.hybridsMade >= currentOrder.hybridCount) return null;

            // Get the next recipe to make
            int index = currentOrder.hybridsMade;
            return currentOrder.hybridRecipes[index];
        }
    }

    public bool HasMoreHybridsToMake
    {
        get
        {
            if (currentOrder == null) return false;
            return currentOrder.hybridsMade < currentOrder.hybridCount;
        }
    }

    public void GenerateOrder(CustomerType customerType)
    {
        // Safety: if you forgot to assign recipes, tell you loudly
        if (hybridRecipe1 == null || hybridRecipe2 == null)
        {
            Debug.LogError("[OrderManager] hybridRecipe1 or hybridRecipe2 is NOT assigned in Inspector.");
            return;
        }

        currentOrder = new CustomerOrder();

        // Random wrap + accessory for all customers
        currentOrder.wrapIcon = PickRandom(wrapOptions);
        currentOrder.accessoryIcon = PickRandom(accessoryOptions);

        // Set what this customer wants
        if (customerType == CustomerType.Customer1)
        {
            currentOrder.hybridRecipes = new RecipeData[] { hybridRecipe1 };
            currentOrder.hybridCount = 1;

            currentOrder.normalCount = 0;
            currentOrder.normalFlower = null;
        }
        else if (customerType == CustomerType.Customer2)
        {
            currentOrder.hybridRecipes = new RecipeData[] { hybridRecipe2 };
            currentOrder.hybridCount = 1;

            currentOrder.normalCount = 1;
            currentOrder.normalFlower = PickRandom(possibleNormalFlowers);
        }
        else // Customer3
        {
            currentOrder.hybridRecipes = new RecipeData[] { hybridRecipe1, hybridRecipe2 };
            currentOrder.hybridCount = 2;

            currentOrder.normalCount = 0;
            currentOrder.normalFlower = null;
        }

        // Progress resets
        currentOrder.hybridsMade = 0;
        currentOrder.normalsPrepared = 0;

        Debug.Log($"[OrderManager] New order: {customerType} | CurrentRecipe={(CurrentRecipe != null ? CurrentRecipe.name : "NULL")}");
    }

    public void MarkHybridCompleted()
    {
        if (currentOrder == null) return;

        currentOrder.hybridsMade++;
        Debug.Log($"[OrderManager] Hybrids made: {currentOrder.hybridsMade}/{currentOrder.hybridCount}");
    }

    public void MarkNormalCompleted()
    {
        if (currentOrder == null) return;

        currentOrder.normalsPrepared++;
        Debug.Log($"[OrderManager] Normals prepared: {currentOrder.normalsPrepared}/{currentOrder.normalCount}");
    }

    // ---------- Helpers ----------

    private T PickRandom<T>(T[] array) where T : Object
    {
        if (array == null || array.Length == 0) return null;
        return array[Random.Range(0, array.Length)];
    }
}
