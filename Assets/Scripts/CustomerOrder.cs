using UnityEngine;

public class CustomerOrder
{
    public int hybridCount;

    // What hybrids the customer wants (results only)
    public FlowerType[] requiredHybrids;

    public int normalCount;
    public FlowerType normalFlower;

    public Sprite wrapIcon;
    public Sprite accessoryIcon;

    public int hybridsMade;
    public int normalsPrepared;

    public bool IsComplete()
    {
        return hybridsMade >= hybridCount && normalsPrepared >= normalCount;
    }
}

