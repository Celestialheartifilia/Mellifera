using UnityEngine;

public class HybridFormulas : MonoBehaviour
{
    // Try to find a recipe result for (first + second).
    // Returns true if a recipe exists.
    public bool TryGetResult(FlowerType first, FlowerType second, out FlowerType result)
    {
        // Default output
        result = FlowerType.Flower1;

        // Recipe 1: Flower1 + Flower2 -> Hybrid1
        if (Matches(first, second, FlowerType.Flower1, FlowerType.Flower2))
        {
            result = FlowerType.Hybrid1;
            return true;
        }

        // Recipe 2: Flower2 + Flower3 -> Hybrid2
        if (Matches(first, second, FlowerType.Flower2, FlowerType.Flower3))
        {
            result = FlowerType.Hybrid2;
            return true;
        }

        // No recipe matched
        return false;
    }

    // Helper: checks (x,y) matches (a,b) OR (b,a)
    private bool Matches(FlowerType x, FlowerType y, FlowerType a, FlowerType b)
    {
        return (x == a && y == b) || (x == b && y == a);
    }
}
