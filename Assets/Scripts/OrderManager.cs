using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderManager : MonoBehaviour
{

    public BeeInteractionDetector bee;   // detects flower + pot
    public HybridFormulas hybridFormulas;        // code-only recipes
    public Button pollinateButton;

    public float holdToGrowSeconds = 1.5f;
    public Animator seedGrowAnimator;    // optional trigger: "Grow"

    private enum State { NeedFirst, NeedSecond, GoToPot }
    private State state = State.NeedFirst;

    private bool hasFirst = false;
    private FlowerType firstType;

    private FlowerType resultType; // what we will grow after pot

    private float holdTimer = 0f;

    void Start()
    {
        if (pollinateButton != null)
            pollinateButton.onClick.AddListener(OnPollinatePressed);

        pollinateButton.interactable = false;
    }

    void Update()
    {
        UpdatePollinateButton();

        if (state == State.GoToPot)
            HandlePotHold();
    }

    void UpdatePollinateButton()
    {
        // Must be touching a flower to pollinate
        Flower touchingFlower = bee.currentFlower;
        if (touchingFlower == null)
        {
            pollinateButton.interactable = false;
            return;
        }

        // Step 1: any flower is OK
        if (state == State.NeedFirst)
        {
            pollinateButton.interactable = true;
            return;
        }

        // Step 2: must match a recipe with the first flower
        if (state == State.NeedSecond)
        {
            FlowerType secondType = touchingFlower.type;

            FlowerType tempResult;
            bool valid = hybridFormulas.TryGetResult(firstType, secondType, out tempResult);

            pollinateButton.interactable = valid;
            return;
        }

        // Pot step: button off
        pollinateButton.interactable = false;
    }

    void OnPollinatePressed()
    {
        // Must be touching a flower
        if (bee.currentFlower == null) return;

        FlowerType currentType = bee.currentFlower.type;

        // First pollination: store the first flower type
        if (state == State.NeedFirst)
        {
            firstType = currentType;
            hasFirst = true;
            state = State.NeedSecond;

            // Optional: force player to move away
            bee.currentFlower = null;

            Debug.Log("[Hybrid] First flower chosen: " + firstType);
            return;
        }

        // Second pollination: check recipe immediately
        if (state == State.NeedSecond)
        {
            FlowerType tempResult;
            bool valid = hybridFormulas.TryGetResult(firstType, currentType, out tempResult);

            if (!valid)
            {
                Debug.Log("[Hybrid] Wrong combination.");
                return;
            }

            // Save result and go to pot stage
            resultType = tempResult;
            state = State.GoToPot;
            holdTimer = 0f;

            Debug.Log("[Hybrid] Recipe OK: " + firstType + " + " + currentType + " -> " + resultType);
        }
    }

    void HandlePotHold()
    {
        // Must be near pot
        if (!bee.nearPot)
        {
            holdTimer = 0f;
            return;
        }

        // Hold space to grow
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

        if (seedGrowAnimator != null)
            seedGrowAnimator.SetTrigger("Grow");

        Debug.Log("[Hybrid] Grown hybrid: " + resultType);

        // For now, reset so you can test again immediately
        ResetStation();
    }

    public void ResetStation()
    {
        state = State.NeedFirst;
        hasFirst = false;
        holdTimer = 0f;
        pollinateButton.interactable = false;
    }
}
