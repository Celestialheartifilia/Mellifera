using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PollinationManager : MonoBehaviour
{
    //reference for the pollination rule guide
    //e.g. normal flower 1 + normal flower 2 = hybrid flower 1
    public HybridRulesSOScript hybridRulesSOScript;

    //Temporarily stores the two normal flowers the player pollinated into a list with a capacity of 2 -> players can only pollinate 2 different flower
    private readonly List<ItemsSOScript> pickedFlowers = new List<ItemsSOScript>(2);

    [Header("VisualIndicators")]
    public GameObject Hybrid1;
    public GameObject Hybrid2;
    public GameObject Hybrid3;

    //Stores the final hybrid result -> Acts as the “output” of pollination
    public ItemsSOScript ReadyHybrid { get; private set; }

    //method to clears partial input/output -> used for Invalid combo/After planting
    public void ResetPollination()
    {
        pickedFlowers.Clear();
        ReadyHybrid = null;
    }

    public bool TryAddPollinatedFlower(ItemsSOScript normalFlower)
    {
        //already have a hybrid ready
        if (ReadyHybrid != null) 
        { 
            return false;
        }
        //already picked 2 normal flowers
        if (pickedFlowers.Count >= 2) 
        {
            return false;
        }
        //prevent picking the same flower twice
        if (pickedFlowers.Contains(normalFlower))
        {
            Debug.Log("Same flower cannot be picked twice.");
            return false;
        }

        //Adds a normal flower to the list
        pickedFlowers.Add(normalFlower);

        //once 2 flower is picked
        if (pickedFlowers.Count == 2)
        {
            //get the hybrid result of the pollination
            ItemsSOScript result = hybridRulesSOScript.GetHybridResult(pickedFlowers[0], pickedFlowers[1]);
            //if there are no results -> the combo is wrong
            if (result == null)
            {
                Debug.Log("Invalid combo. Resetting.");
                //reset
                ResetPollination();
                return false;
            }

            //if have results -> store the result in ReadyHybrid
            ReadyHybrid = result;
            Debug.Log($"[POLLINATION] Hybrid ready: {ReadyHybrid.itemName}");
        }

        return true;
    }

    //passes the data of the hybrid flower to the pot
    public bool TryPlantInto(Pot pot)
    {
        //checks if there is the data stored
        if (ReadyHybrid == null) 
        {
            return false;
        }

        // Let the pot decide if planting is valid
        bool planted = pot.Plant(ReadyHybrid);

        if (!planted)
        {
            return false;
        }
            
        //pollination is resetted
        ResetPollination();
        return true;
    }
}
