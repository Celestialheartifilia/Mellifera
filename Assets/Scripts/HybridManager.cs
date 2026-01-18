using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HybridManager : MonoBehaviour
{
    public OrderManager orderManager;              // holds currentOrder -> requiredRecipe

    public BeeInteractionDetector bee;               // has currentFlower + nearPot

    public Button pollinateButton;                 // click to pollinate
    public Text infoText;                          // instructions text (TMP is fine too)

    public float holdToGrowSeconds = 1.5f;         // how long to hold Space at pot
    public Animator seedGrowAnimator;              // optional: trigger "Grow"

    // These are the steps of your hybrid station.
    private enum State
    {
        NeedFirstFlower,     // pollinate 1/2
        NeedSecondFlower,    // pollinate 2/2 (must match recipe)
        GoToPot,             // go to pot and hold Space
        Done                 // grown (next: cutting minigame)
    }

    private State state = State.NeedFirstFlower;

    private FlowerData firstFlowerData;            // remember what we picked first
    private float holdTimer = 0f;                  // for holding Space at the pot

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Link the UI button to our pollinate function
        if (pollinateButton != null)
            pollinateButton.onClick.AddListener(OnPollinatePressed);

        RefreshUI();
    }

    // Update is called once per frame
    void Update()
    {
        // 1) If there is no active order or recipe, hybrid station should do nothing.
        RecipeData currentRecipe = GetCurrentRecipe();
        if (currentRecipe == null)
        {
            SetInfo("No active order. Go take an order first.");
            SetPollinateInteractable(false);
            return;
        }

        // 2) Update whether the pollinate button is clickable (greyed out vs lit up)
        UpdatePollinateButton(currentRecipe);

        // 3) If we are at the pot step, handle holding Space to grow
        if (state == State.GoToPot)
            HandlePotHold(currentRecipe);
    }

    // --------- Core logic ---------

    // Gets the current order's recipe.
    // This is the whole point: hybrid station validates ONLY this recipe, not a big list.
    private RecipeData GetCurrentRecipe()
    {
        if (orderManager == null) return null;
        return orderManager.CurrentRecipe;
    }

    // Grey out / light up pollinate button based on what step we are in and what bee is touching.
    private void UpdatePollinateButton(RecipeData recipe)
    {
        // Bee must be touching a flower for pollination to even be possible
        Flower currentFlower = bee != null ? bee.currentFlower : null;
        FlowerData currentData = (currentFlower != null) ? currentFlower.data : null;

        bool canPollinate = false;

        if (state == State.NeedFirstFlower)
        {
            // Any flower is allowed for the first pollination.
            canPollinate = (currentData != null);
        }
        else if (state == State.NeedSecondFlower)
        {
            // Second pollination must complete the recipe the customer asked for.
            // Also prevents "pollinate the same flower twice" automatically, because recipe won't match.
            if (firstFlowerData != null && currentData != null)
                canPollinate = recipe.Matches(firstFlowerData, currentData);
        }
        else
        {
            // At pot / done stage, pollinate button should be disabled.
            canPollinate = false;
        }

        SetPollinateInteractable(canPollinate);
    }

    // Called when player presses the pollinate button.
    private void OnPollinatePressed()
    {
        RecipeData recipe = GetCurrentRecipe();
        if (recipe == null) return;

        // Bee must be touching a flower
        Flower currentFlower = bee != null ? bee.currentFlower : null;
        if (currentFlower == null || currentFlower.data == null) return;

        FlowerData currentData = currentFlower.data;

        // Step 1: store first flower
        if (state == State.NeedFirstFlower)
        {
            firstFlowerData = currentData;
            state = State.NeedSecondFlower;

            SetInfo($"Pollinate 2/2: Find the flower that matches the order recipe.");
            return;
        }

        // Step 2: validate second flower against CURRENT order recipe
        if (state == State.NeedSecondFlower)
        {
            bool valid = recipe.Matches(firstFlowerData, currentData);
            if (!valid)
            {
                // Usually the button would be greyed out if invalid,
                // but this is here as a safety net.
                SetInfo("Wrong combo. Try a different flower.");
                return;
            }

            // Recipe confirmed, move to pot step
            state = State.GoToPot;
            holdTimer = 0f;

            SetInfo("Recipe confirmed. Go to pot and HOLD Space to grow.");
            return;
        }
    }

    // Step 3: hold space at the pot to grow
    private void HandlePotHold(RecipeData recipe)
    {
        bool nearPot = (bee != null && bee.nearPot);

        // If not near pot, keep instructing player
        if (!nearPot)
        {
            holdTimer = 0f;
            SetInfo("Go to pot and HOLD Space to grow.");
            return;
        }

        // Near pot: hold space to fill timer
        if (Input.GetKey(KeyCode.Space))
        {
            holdTimer += Time.deltaTime;

            float remaining = Mathf.Max(0f, holdToGrowSeconds - holdTimer);
            SetInfo($"Growing... {remaining:0.0}s");

            // Done growing
            if (holdTimer >= holdToGrowSeconds)
            {
                CompleteGrow(recipe);
            }
        }
        else
        {
            // Letting go resets. (You can change this to "decay" if you want.)
            holdTimer = 0f;
            SetInfo("At pot: HOLD Space to grow.");
        }
    }

    // Finish the grow step, trigger animation, and mark "Done"
    private void CompleteGrow(RecipeData recipe)
    {
        holdTimer = 0f;

        if (seedGrowAnimator != null)
            seedGrowAnimator.SetTrigger("Grow"); // Make sure your Animator has a Trigger called "Grow"

        state = State.Done;

        // This is the item you should hand to the next step (cutting/inventory)
        FlowerData resultHybrid = recipe.resultHybrid;

        // For now we just show text. Next you will start cutting minigame here.
        string resultName = (resultHybrid != null) ? resultHybrid.displayName : "Unknown Hybrid";
        SetInfo($"Grown: {resultName}. Next: Cutting minigame.");

        // If you want: call a method like StartCutting(resultHybrid) here later.
    }

    // --------- UI helpers ---------

    private void RefreshUI()
    {
        switch (state)
        {
            case State.NeedFirstFlower:
                SetInfo("Pollinate 1/2: Touch any flower and press Pollinate.");
                break;

            case State.NeedSecondFlower:
                SetInfo("Pollinate 2/2: Touch the correct flower (recipe) and press Pollinate.");
                break;

            case State.GoToPot:
                SetInfo("Go to pot: HOLD Space to grow.");
                break;

            case State.Done:
                // message is set when completed
                break;
        }
    }

    private void SetInfo(string msg)
    {
        if (infoText != null)
            infoText.text = msg;
    }

    private void SetPollinateInteractable(bool on)
    {
        if (pollinateButton != null)
            pollinateButton.interactable = on;
    }

    // Call this after cutting + collecting so you can do another hybrid.
    public void ResetStation()
    {
        state = State.NeedFirstFlower;
        firstFlowerData = null;
        holdTimer = 0f;
        RefreshUI();
    }
}

