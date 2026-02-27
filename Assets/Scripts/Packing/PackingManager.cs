using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PackingManager : MonoBehaviour
{
    [Header("Hybrid Flowers (Gameplay)")]
    public GameObject hybridFlower1;
    public GameObject hybridFlower2;

    [Header("Hybrid Buttons (UI)")]
    public Button hybridButton1;
    public Button hybridButton2;

    [Header("Wrap Visual")]
    public SpriteRenderer wrapBackRenderer;
    public SpriteRenderer wrapFrontRenderer;
    public ItemsSOScript wrap1;
    public ItemsSOScript wrap2;
    public Sprite wrap1BackSprite;
    public Sprite wrap1FrontSprite;

    public Sprite wrap2BackSprite;
    public Sprite wrap2FrontSprite;

    [Header("Accessory Visual")]
    public ItemsSOScript accessory1;
    public ItemsSOScript accessory2;
    public GameObject accessory1Object;
    public GameObject accessory2Object;

    [Header("Wrap + Accessory Buttons")]
    public GameObject bgForAccessories;
    public Button wrap1Button;
    public Button wrap2Button;
    public Button accessory1Button;
    public Button accessory2Button;

    [Header("Order")]
    public Button orderCompleteButton;

    ItemsSOScript collectedHybrid;
    ItemsSOScript selectedWrap;
    ItemsSOScript selectedAccessory;

    [Header("UI Order")]
    [SerializeField] GameObject CorrectOrderPrompt;
    [SerializeField] GameObject WrongOrderPrompt;

    bool flowerSelected = false;
    bool wrapSelected = false;
    bool accessorySelected = false;


    bool leavesPlucked = false;

    void Start()
    {
        // Safety
        if (GameState.Instance == null || GameState.Instance.collectedHybrids.Count == 0)
        {
            Debug.LogError("No collected hybrid data found.");
            return;
        }

        collectedHybrid = GameState.Instance.collectedHybrids[0];

        // Hide flowers first
        hybridFlower1.SetActive(false);
        hybridFlower2.SetActive(false);

        hybridButton1.gameObject.SetActive(false);
        hybridButton2.gameObject.SetActive(false);

        //Hide Order Prompt first
        CorrectOrderPrompt.SetActive(false);
        WrongOrderPrompt.SetActive(false);

        // Disable wrap + accessory UI initially
        SetWrapButtons(false);
        SetAccessoryButtons(false);
        orderCompleteButton.interactable = false;

        // Determine which hybrid button to show
        var tag1 = hybridFlower1.GetComponent<HybridFlowerTag>();
        var tag2 = hybridFlower2.GetComponent<HybridFlowerTag>();

        if (tag1 == null || tag2 == null)
        {
            Debug.LogError("HybridFlowerTag missing on flower GameObjects.");
            return;
        }

        // Decide which button to show
        if (tag1.flowerItemData == collectedHybrid)
        {
            hybridButton1.gameObject.SetActive(true);
            hybridButton1.onClick.AddListener(() => ActivateHybridFlower(hybridFlower1));
        }
        else if (tag2.flowerItemData == collectedHybrid)
        {
            hybridButton2.gameObject.SetActive(true);
            hybridButton2.onClick.AddListener(() => ActivateHybridFlower(hybridFlower2));
        }
        else
        {
            Debug.LogError("Collected hybrid does not match any flower.");
        }
    }

    void ActivateHybridFlower(GameObject flower)
    {
        if (wrapSelected)
        {
            Debug.Log("Dispose current wrap first!");
            // show popup here
            return;
        }

        flowerSelected = true;

        // Hide buttons after selection
        hybridButton1.gameObject.SetActive(false);
        hybridButton2.gameObject.SetActive(false);

        // Activate gameplay object
        flower.SetActive(true);
    }

    // CALL THIS from your leaf plucking script when done
    public void OnLeavesPlucked()
    {
        leavesPlucked = true;
        bgForAccessories.gameObject.SetActive(true);

        SetWrapButtons(true);

        // DO NOT enable accessories yet
        SetAccessoryButtons(false);
    }

    void SetWrapButtons(bool state)
    {
        wrap1Button.gameObject.SetActive(state);
        wrap2Button.gameObject.SetActive(state);
    }

    void SetAccessoryButtons(bool state)
    {
        accessory1Button.gameObject.SetActive(state);
        accessory2Button.gameObject.SetActive(state);
    }

    // WRAP SELECTION
    public void SelectWrap1()
    {
        selectedWrap = wrap1;
        wrapSelected = true;

        wrapBackRenderer.sprite = wrap1BackSprite;
        wrapFrontRenderer.sprite = wrap1FrontSprite;

        SetAccessoryButtons(true);

        CheckIfOrderReady();
    }

    public void SelectWrap2()
    {
        selectedWrap = wrap2;
        wrapSelected = true;

        wrapBackRenderer.sprite = wrap2BackSprite;
        wrapFrontRenderer.sprite = wrap2FrontSprite;

        SetAccessoryButtons(true);

        CheckIfOrderReady();
    }

    // ACCESSORY SELECTION
    public void SelectAccessory1()
    {
        if (!wrapSelected)
            return;

        selectedAccessory = accessory1;
        accessorySelected = true;


        accessory1Object.SetActive(true);
        accessory2Object.SetActive(false);

        CheckIfOrderReady();
    }

    public void SelectAccessory2()
    {
        if (!wrapSelected)
            return;

        selectedAccessory = accessory2;
        accessorySelected = true;

        accessory1Object.SetActive(false);
        accessory2Object.SetActive(true);

        CheckIfOrderReady();
    }

    void CheckIfOrderReady()
    {
        if (selectedWrap != null && selectedAccessory != null)
        {
            orderCompleteButton.interactable = true;
        }
    }

    public void OnOrderComplete()
    {
        ValidateOrder();
    }


    void ValidateOrder()
    {
        var order = OrderTakingManager.Instance.currentOrder;

        bool flowerCorrect = order.orderedItems.Contains(collectedHybrid);
        bool wrapCorrect = order.orderedItems.Contains(selectedWrap);
        bool accessoryCorrect = order.orderedItems.Contains(selectedAccessory);

        if (flowerCorrect && wrapCorrect && accessoryCorrect)
        {
            Debug.Log("Order completed successfully!");
            CorrectOrderPrompt.SetActive(true);
        }
        else
        {
            Debug.Log("Order incorrect!");
            WrongOrderPrompt.SetActive(true);

        }


    }

    public void HandleDisposal(GameObject disposed)
    {
        // Accessory
        if (disposed == accessory1Object || disposed == accessory2Object)
        {
            accessorySelected = false;
            selectedAccessory = null;

            disposed.SetActive(false);
            return;
        }

        // Wrap
        if (disposed == wrapBackRenderer.gameObject ||
            disposed == wrapFrontRenderer.gameObject)
        {
            wrapSelected = false;
            selectedWrap = null;

            wrapBackRenderer.sprite = null;
            wrapFrontRenderer.sprite = null;
            return;
        }

        // Flower
        if (disposed == hybridFlower1 || disposed == hybridFlower2)
        {
            disposed.SetActive(false);
            ResetPackingScene();
        }
    }

    void ResetToFlowerState()
    {
        flowerSelected = false;
        leavesPlucked = false;

        wrapSelected = false;
        selectedWrap = null;
        wrapBackRenderer.sprite = null;
        wrapFrontRenderer.sprite = null;

        accessorySelected = false;
        selectedAccessory = null;
        accessory1Object.SetActive(false);
        accessory2Object.SetActive(false);

        SetWrapButtons(false);
        SetAccessoryButtons(false);
        bgForAccessories.SetActive(false);

        orderCompleteButton.interactable = false;

        hybridButton1.gameObject.SetActive(true);
        hybridButton2.gameObject.SetActive(true);
    }

    void ResetPackingScene()
    {
        // ===== STATE =====
        flowerSelected = false;
        wrapSelected = false;
        accessorySelected = false;
        leavesPlucked = false;

        selectedWrap = null;
        selectedAccessory = null;

        // ===== FLOWER =====
        hybridFlower1.SetActive(false);
        hybridFlower2.SetActive(false);

        hybridButton1.gameObject.SetActive(false);
        hybridButton2.gameObject.SetActive(false);

        // ===== LEAVES RESET =====
        ResetLeaves();

        // ===== WRAP RESET =====
        wrapBackRenderer.sprite = null;
        wrapFrontRenderer.sprite = null;

        SetWrapButtons(false);
        SetAccessoryButtons(false);

        bgForAccessories.SetActive(false);

        // ===== ACCESSORY RESET =====
        accessory1Object.SetActive(false);
        accessory2Object.SetActive(false);

        // ===== ORDER =====
        orderCompleteButton.interactable = false;

        Debug.Log("Packing scene fully reset");
    }

    void ResetLeaves()
    {
        LeafTracker tracker = FindObjectOfType<LeafTracker>();
        if (tracker != null)
        {
            tracker.ResetLeaves();
        }
    }
}


