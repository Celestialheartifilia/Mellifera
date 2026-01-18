using UnityEngine;

[CreateAssetMenu(fileName = "RecipeData", menuName = "Scriptable Objects/RecipeData")]
public class RecipeData : ScriptableObject
{
    [Header("Inputs (two flowers)")]
    public FlowerData flowerA;
    public FlowerData flowerB;

    [Header("Result (the hybrid flower you get)")]
    public FlowerData resultHybrid;

    // Checks if 2 flowers match this recipe. Order does not matter.
    public bool Matches(FlowerData x, FlowerData y)
    {
        if (x == null || y == null) return false;
        if (flowerA == null || flowerB == null) return false;

        return (x == flowerA && y == flowerB) ||
               (x == flowerB && y == flowerA);
    }
}
