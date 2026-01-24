using UnityEngine;

public class Pot : MonoBehaviour
{
    public bool isEmpty = true;
    public ItemsSOScript plantedHybrid;

    public void Plant(ItemsSOScript hybrid)
    {
        isEmpty = false;
        plantedHybrid = hybrid;

        Debug.Log($"Planted: {hybrid.name}");
        // Later: show planted sprite, enable fertilizer UI, etc.
    }
}
