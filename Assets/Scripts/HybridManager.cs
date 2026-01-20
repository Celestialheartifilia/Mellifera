using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HybridManager : MonoBehaviour
{
    [Header("Scene References (drag these in Inspector)")]
    public BeeInteractionDetector bee;       // detects currentFlower + nearPot
    public HybridFormulas formulas;          // your code-only recipe checker
    public Button pollinateButton;           // UI button

    [Header("Grow Settings")]
    public float holdToGrowSeconds = 1.5f;
    public Animator seedGrowAnimator;        // optional, Trigger name: "Grow"

    // Steps of the hybrid station
    private enum Step
    {
        PickFirstFlower,   // pollinate 1/2
        PickSecondFlower,  // pollinate 2/2
        GrowAtPot          // hold space at pot
    }

    private Step step = Step.PickFirstFlower;

    // Stored info for the current hybrid attempt
    private bool hasFirstFlower = false;
    private FlowerType firstFlowerType;
    private FlowerType resultHybridType;

    // Pot hold timer
    private float holdTimer = 0f;

    void Start()
    {
        // Safety checks (so you don't silently suffer)
        if (bee == null) Debug.LogError("[HybridManager] BeeInteractionDetector reference missing.");
        if (formulas == null) Debug.LogError("[HybridManager] HybridFormulas reference missing.");
        if (pollinateButton == null) Debug.LogError("[HybridManager] Pollinate Button reference missing.");

        // Hook up button click
        if (pollinateButton != null)
            pollinateButton.onClick.AddListener(OnPollinateClicked);

        // Start with button off until we touch a flower
        SetPollinateButton(false);
    }

    void Update()
    {
        // 1) Update button interactable state every frame
        UpdatePollinateButton();

        // 2) Handle pot growing only when we reached that step
        if (step == Step.GrowAtPot)
            UpdateGrowAtPot();
    }

    // ---------------------------
    // BUTTON ENABLE/DISABLE LOGIC
    // ---------------------------
    void UpdatePollinateButton()
    {
        // If missing refs, keep it off
        if (bee == null || formulas == null || pollinateButton == null)
        {
            SetPollinateButton(false);
            return;
        }

        // If not touching a flower, cannot pollinate
        Flower touchingFlower = bee.currentFlower;
        if (touchingFlower == null)
        {
            SetPollinateButton(false);
            return;
        }

        // Step 1: any flower is fine
        if (step == Step.PickFirstFlower)
        {
            SetPollinateButton(true);
            return;
        }

        // Step 2: only enable if the combo matches a valid formula
        if (step == Step.PickSecondFlower)
        {
            if (!hasFirstFlower)
            {
                SetPollinateButton(false);
                return;
            }

            FlowerType secondType = touchingFlower.type;

            FlowerType tempResult;
            bool valid = formulas.TryGetResult(firstFlowerType, secondType, out tempResult);

            SetPollinateButton(valid);
            return;
        }

        // Step 3: growing at pot, pollinate button is off
        SetPollinateButton(false);
    }

    // ---------------------------
    // WHEN PLAYER CLICKS POLLINATE
    // ---------------------------
    void OnPollinateClicked()
    {
        if (bee == null || formulas == null) return;
        if (bee.currentFlower == null) return;

        FlowerType currentType = bee.currentFlower.type;

        // Step 1: store the first flower type
        if (step == Step.PickFirstFlower)
        {
            firstFlowerType = currentType;
            hasFirstFlower = true;
            step = Step.PickSecondFlower;

            // Optional: force player to move away so they can't double-click same flower
            bee.currentFlower = null;

            Debug.Log("[HybridManager] First flower selected: " + firstFlowerType);
            return;
        }

        // Step 2: validate the combo and store the result
        if (step == Step.PickSecondFlower)
        {
            FlowerType tempResult;
            bool valid = formulas.TryGetResult(firstFlowerType, currentType, out tempResult);

            if (!valid)
            {
                Debug.Log("[HybridManager] Invalid combination: " + firstFlowerType + " + " + currentType);
                return;
            }

            resultHybridType = tempResult;
            step = Step.GrowAtPot;
            holdTimer = 0f;

            Debug.Log("[HybridManager] Combo OK: " + firstFlowerType + " + " + currentType + " -> " + resultHybridType);
        }
    }

    // ---------------------------
    // POT HOLD TO GROW
    // ---------------------------
    void UpdateGrowAtPot()
    {
        // Must be touching pot
        if (!bee.nearPot)
        {
            holdTimer = 0f;
            return;
        }

        // Hold Space to grow
        if (Input.GetKey(KeyCode.Space))
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdToGrowSeconds)
                FinishGrow();
        }
        else
        {
            // Let go = reset timer
            holdTimer = 0f;
        }
    }

    void FinishGrow()
    {
        holdTimer = 0f;

        // Play grow animation (optional)
        if (seedGrowAnimator != null)
            seedGrowAnimator.SetTrigger("Grow");

        Debug.Log("[HybridManager] Grown hybrid result: " + resultHybridType);

        // TODO next:
        // - send resultHybridType to cutting minigame
        // - add to inventory

        // For now: reset so you can test again
        ResetStation();
    }

    // ---------------------------
    // RESET
    // ---------------------------
    public void ResetStation()
    {
        step = Step.PickFirstFlower;
        hasFirstFlower = false;
        holdTimer = 0f;

        SetPollinateButton(false);
    }

    void SetPollinateButton(bool on)
    {
        if (pollinateButton != null)
            pollinateButton.interactable = on;
    }


}
