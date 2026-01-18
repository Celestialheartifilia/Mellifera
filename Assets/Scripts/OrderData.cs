using UnityEngine;

[CreateAssetMenu(fileName = "OrderData", menuName = "Scriptable Objects/OrderData")]
public class OrderData : ScriptableObject
{
    public RecipeData requiredRecipe;

    // placeholders for later
    public Sprite wrapIcon;
    public Sprite accessoryIcon;
}
