using UnityEngine;
using UnityEngine.UI;

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

        // Disable wrap + accessory UI initially
        SetWrapAccessoryButtons(false);
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
        SetWrapAccessoryButtons(true);
    }

    void SetWrapAccessoryButtons(bool state)
    {
        wrap1Button.gameObject.SetActive(state);
        wrap2Button.gameObject.SetActive(state);
        accessory1Button.gameObject.SetActive(state);
        accessory2Button.gameObject.SetActive(state);
    }

    // WRAP SELECTION
    public void SelectWrap1()
    {
        selectedWrap = wrap1;

        wrapBackRenderer.sprite = wrap1BackSprite;
        wrapFrontRenderer.sprite = wrap1FrontSprite;

        CheckIfOrderReady();
    }

    public void SelectWrap2()
    {
        selectedWrap = wrap2;

        wrapBackRenderer.sprite = wrap2BackSprite;
        wrapFrontRenderer.sprite = wrap2FrontSprite;

        CheckIfOrderReady();
    }

    // ACCESSORY SELECTION
    public void SelectAccessory1()
    {
        selectedAccessory = accessory1;

        accessory1Object.SetActive(true);
        accessory2Object.SetActive(false);

        CheckIfOrderReady();
    }

    public void SelectAccessory2()
    {
        selectedAccessory = accessory2;

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
        }
        else
        {
            Debug.Log("Order incorrect!");
        }
    }

}
