using System.Collections.Generic;
using UnityEngine;

public class PollinationManager : MonoBehaviour
{
    public HybridRulesSOScript hybridRulesSOScript;

    private readonly List<ItemsSOScript> pickedFlowers = new List<ItemsSOScript>(2);
    public ItemsSOScript ReadyHybrid { get; private set; }

    public void ResetPollination()
    {
        pickedFlowers.Clear();
        ReadyHybrid = null;
    }

    public bool TryAddPollinatedFlower(ItemsSOScript normalFlower)
    {
        if (ReadyHybrid != null) return false;          // already have a hybrid ready
        if (pickedFlowers.Count >= 2) return false;      // already picked 2

        pickedFlowers.Add(normalFlower);

        if (pickedFlowers.Count == 2)
        {
            ItemsSOScript result = hybridRulesSOScript.GetHybridResult(pickedFlowers[0], pickedFlowers[1]);
            if (result == null)
            {
                Debug.Log("Invalid combo. Resetting.");
                ResetPollination();
                return false;
            }

            ReadyHybrid = result;
            Debug.Log($"Hybrid ready: {ReadyHybrid.name}");
        }

        return true;
    }

    public bool TryPlantInto(Pot pot)
    {
        if (ReadyHybrid == null) return false;
        if (pot.growthState != Pot.FlowerGrowthState.Empty) return false;

        pot.Plant(ReadyHybrid);
        ResetPollination();
        return true;
    }
}
