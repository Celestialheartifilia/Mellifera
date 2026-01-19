using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
    // What the customer wants
    public RecipeData[] hybridRecipes;
    public int hybridCount;
    public int hybridsMade;

    public int normalCount;                // 0 or 1 for your design
    public FlowerData normalFlower;        // which normal flower (if needed)

    // Packing wants (random every order)
    public Sprite wrapIcon;
    public Sprite accessoryIcon;

    // Progress tracking (what player already made)
    public int normalsPrepared;

    // Helper: is the whole order done?
    public bool IsComplete()
    {
        return hybridsMade >= hybridCount && normalsPrepared >= normalCount;
    }
}
