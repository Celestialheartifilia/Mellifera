using UnityEngine;

public class Pot : MonoBehaviour
{
    [Header("Tool Colliders")]
    public Collider2D fertiliserCollider;
    public Collider2D scissorsCollider;
    public GameObject BeeGone;

    [Header("UI (Optional)")]
    public GameObject toolsReadyPopup;

    //public bool isEmpty = true;
    public enum FlowerGrowthState
    {
        Empty,
        Planted,
        Fertilised,
        Grown
    }

    //store the current state -> in this case, there is no flower = empty
    [HideInInspector] public FlowerGrowthState growthState = FlowerGrowthState.Empty;

    //reference to the hybridflowermanager -> for communication between pot n manager
    public HybridFlowerManager hybridFlowerManager;

    //to store the hybrid that was planted
    public ItemsSOScript plantedHybrid;

    //initial empty state
    void Start()
    {
        ResetPot();
    }

    public void ResetPot()
    {
        //disables the fertiliser
        if (fertiliserCollider != null)
        {
            fertiliserCollider.enabled = false;
        }
        //disables the scissors
        if (scissorsCollider != null)
        {
            scissorsCollider.enabled = false;
        }
        //set the bee to active
        if (BeeGone != null)
        {
            BeeGone.SetActive(true);
        }
        //hides UI for tools ready pop up
        if (toolsReadyPopup != null)
        {
            toolsReadyPopup.SetActive(false);
        }

        Debug.Log("[POT] Reset.");
    }

    //method is used in the pollination manager script
    public bool Plant(ItemsSOScript hybrid)
    {
        //ensures that theres a hybrid to plant
        if (hybrid == null)
        {
            Debug.LogWarning("[POT] Tried to plant NULL hybrid.");
            return false;
        }
        //ensures the pot is empty
        if (growthState != FlowerGrowthState.Empty)
        {
            Debug.LogWarning("[POT] Tried to plant but pot is not empty.");
            return false;
        }

        //stores the hybrid data
        plantedHybrid = hybrid;
        //changes the state to planted
        growthState = FlowerGrowthState.Planted;

        Debug.Log($"[PLANT] Planted hybrid: {hybrid.itemName} | ID: {hybrid.itemID}");

        // Enable fertiliser colliders
        if (fertiliserCollider != null)
        {
            fertiliserCollider.enabled = true;
        }

        //if planting is successful
        return true;

        
    }
    //method is used in FertilizerGrowFlower script
    public bool Fertilise()
    {
        //ensure state is in planted
        if (growthState != FlowerGrowthState.Planted)
        {
            Debug.LogWarning("[POT] Fertilise failed — wrong state.");
            return false;
        }
        //changes the state to fertilised
        growthState = FlowerGrowthState.Fertilised;
        Debug.Log("[FERTILISE] Fertilizer applied");

        //add anim here, then grow method
        Grow();
        //if fertilise is successful
        return true;
    }

    public void Grow()
    {
        //ensure state is in fertilised
        if (growthState != FlowerGrowthState.Fertilised)
        {
            return;
        }
        //changes the state to grown
        growthState = FlowerGrowthState.Grown;
        Debug.Log("[GROW] Flower fully grown");

        // Enable fertiliser + scissors colliders
        //if (fertiliserCollider != null)
        //{
        //    fertiliserCollider.enabled = true;
        //} 

        if (scissorsCollider != null)
        {
            scissorsCollider.enabled = true;
        }

        if (BeeGone != null)
        {
            BeeGone.SetActive(false);
        }

        // Optional popup
        if (toolsReadyPopup != null)
        {
            toolsReadyPopup.SetActive(true);
        }

        if (hybridFlowerManager != null)
        {
            //Notifies manager with hybrid data only
            hybridFlowerManager.OnHybridReadyToCut(plantedHybrid);
        }
    }

    // Add Code so that when done the fertilizer and scissors can use maybe a ui Pop up 
    public bool IsReadyToFertilise()
    {
        return growthState == FlowerGrowthState.Planted;
    }
}
