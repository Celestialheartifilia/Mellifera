using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HybridManager : MonoBehaviour
{
    //references
    public OrderManager orderManager;          // provides the current recipe
    public BeeInteractionDetector bee;         // tells us what flower/pot bee is touching

    public Button pollinateButton;

    public float holdToGrowSeconds = 1.5f;
    public Animator seedGrowAnimator;          // Trigger name: "Grow"

    private enum State 
    {
        NeedFirstFlower, 
        NeedSecondFlower, 
        GoToPot, 
        Done 
    
    }

    private State state = State.NeedFirstFlower;

    private FlowerData firstFlower;
    private float holdTimer;

    void Start()
    {

        if (pollinateButton != null)
            pollinateButton.onClick.AddListener(OnPollinatePressed);
            Debug.Log("HybridManager started and listener added");

        SetPollinateInteractable(false);
    }


    void Update()
    {

        ////Get current recipe from OrderManager
        //RecipeData recipe = GetCurrentRecipe();
        //if (recipe == null)
        //{
        //    // No recipe means "no order yet" OR not assigned in inspector
        //    SetPollinateInteractable(false);
        //    return;
        //}

        RecipeData recipe = GetCurrentRecipe();

        string recipeName = recipe != null ? recipe.name : "NULL";
        string flowerName = (bee != null && bee.currentFlower != null) ? bee.currentFlower.name : "NONE";
        string flowerDataName = (bee != null && bee.currentFlower != null && bee.currentFlower.data != null) ? bee.currentFlower.data.name : "NULL_DATA";

        Debug.Log($"[Hybrid] recipe={recipeName} | flower={flowerName} | flowerData={flowerDataName} | state={state}");

        //Enable/disable pollinate button
        UpdatePollinateButton(recipe);

        //Grow step
        if (state == State.GoToPot)
            HandlePotHold();
    }

    RecipeData GetCurrentRecipe()
    {
        if (orderManager == null) return null;
        return orderManager.CurrentRecipe;
    }

    void UpdatePollinateButton(RecipeData recipe)
    {
        if (pollinateButton == null) return;

        Flower currentFlower = bee.currentFlower;

        FlowerData currentData = null;
        if (currentFlower != null)
        {
            currentData = currentFlower.data;
        }

        bool canClick = false;

        if (state == State.NeedFirstFlower)
        {
            // Any flower allowed for first pollination
            canClick = (currentData != null);
        }
        else if (state == State.NeedSecondFlower)
        {
            // Second pollination must match the current recipe
            if (firstFlower != null && currentData != null)
            {
                canClick = recipe.Matches(firstFlower, currentData);
            }
        }

        pollinateButton.interactable = canClick;
    }

    void OnPollinatePressed()
    {
        Debug.Log("POLLINATE CLICKED");
        RecipeData recipe = GetCurrentRecipe();
        if (recipe == null) return;

        Flower currentFlower = bee.currentFlower;

        FlowerData currentData = null;
        if (currentFlower != null)
        {
            currentData = currentFlower.data;
        }
        if (currentData == null) return;

        if (state == State.NeedFirstFlower)
        {
            firstFlower = currentData;
            state = State.NeedSecondFlower;

            // Optional: force player to move to a new flower
            bee.currentFlower = null;
            return;
        }

        if (state == State.NeedSecondFlower)
        {
            bool valid = recipe.Matches(firstFlower, currentData);
            if (!valid) return;

            state = State.GoToPot;
            holdTimer = 0f;
        }
    }

    void HandlePotHold()
    {
        if (!bee.nearPot)
        {
            holdTimer = 0f;
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdToGrowSeconds)
                CompleteGrow();
        }
        else
        {
            holdTimer = 0f;
        }
    }

    void CompleteGrow()
    {
        holdTimer = 0f;
        state = State.Done;

        if (seedGrowAnimator != null)
            seedGrowAnimator.SetTrigger("Grow");

        if (orderManager != null)
            orderManager.MarkHybridCompleted();

        Debug.Log("GROW COMPLETE (Order recipe version)");

        if (orderManager != null && orderManager.HasMoreHybridsToMake)
        {
            // Customer wants another hybrid (Customer 3 case)
            ResetStation();
        }
        else
        {
            // No more hybrids needed
            // Next step: cutting / packing / delivery
            Debug.Log("ALL HYBRIDS DONE FOR THIS CUSTOMER");
        }

    }

    void SetPollinateInteractable(bool on)
    {
        if (pollinateButton != null)
            pollinateButton.interactable = on;
    }

    public void ResetStation()
    {
        state = State.NeedFirstFlower;
        firstFlower = null;
        holdTimer = 0f;
    }
}

