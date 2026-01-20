using UnityEngine;

public class CustomerOrder
{
    public FlowerType[] requiredHybrids;
    public bool normalRequired;
    public FlowerType requiredNormal;

    //Packing visuals
    public Sprite wrapIcon;
    public Sprite accessoryIcon;

    //How many hybrids the player has completed
    public int hybridsMade;

    //Whether the normal flower is done
    public bool normalDone;

    public bool IsComplete()
    {
        bool hybridsFinished = hybridsMade >= requiredHybrids.Length;
        bool normalFinished = !normalRequired || normalDone;

        return hybridsFinished && normalFinished;
    }
}

