using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PackingManager : MonoBehaviour
{
    [Header("Hybrid Flowers (Gameplay)")]
    public GameObject hybridFlower1;
    public GameObject hybridFlower2;

    [Header("Hybrid Buttons (UI)")]
    public GameObject flowerTabBackground;
    public Button hybridButton1;
    public Button hybridButton2;

    [Header("Wrap Visual")]
    public SpriteRenderer wrapBackRenderer;
    public SpriteRenderer wrapFrontRenderer;
    public ItemsSOScript wrap1;
    public ItemsSOScript wrap2;
    public Sprite wrap1BackSprite;
    public Sprite wrap1FrontSprite;
    public GameObject wrap;

    public Sprite wrap2BackSprite;
    public Sprite wrap2FrontSprite;

    [Header("Accessory Visual")]
    public ItemsSOScript accessory1;
    public ItemsSOScript accessory2;
    public GameObject accessory1Object;
    public GameObject accessory2Object;

    [Header("Wrap + Accessory Buttons")]
    public GameObject wrapAccessoryTabBackground;
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

    Vector3 accessory1StartPos;
    Vector3 accessory2StartPos;


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

        //store accessory start position
        accessory1StartPos = accessory1Object.transform.localPosition;
        accessory2StartPos = accessory2Object.transform.localPosition;

        // Hide flowers first
        hybridFlower1.SetActive(false);
        hybridFlower2.SetActive(false);
        hybridButton1.gameObject.SetActive(false);
        hybridButton2.gameObject.SetActive(false);

        //Hide Order Prompt first
        CorrectOrderPrompt.SetActive(false);
        WrongOrderPrompt.SetActive(false);

        // Disable wrap + accessory UI initially
        wrapAccessoryTabBackground.gameObject.SetActive(false);
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

        //DECIDE WHICH HYBRID FLOWER ICON BUTTONS TO SHOW UP IN THE FLOWER TAB
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
    //CHECK IF ALL LEAVES ON HYBRID FLOWER ARE PLUCKED, THEN SET WRAPACCESSORY TAB + WRAP ICON BUTTONS TO SHOW
    public void OnLeavesPlucked()
    {
        leavesPlucked = true;
        wrapAccessoryTabBackground.gameObject.SetActive(true);

        SetWrapButtons(true);

        //ACCESSORY ICON BUTTONS ARE NOT SHOWN YET
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

        hybridFlower1.GetComponent<DragReturn>().enabled = false;
        hybridFlower2.GetComponent<DragReturn>().enabled = false;

        wrapBackRenderer.sprite = wrap1BackSprite;
        wrapFrontRenderer.sprite = wrap1FrontSprite;

        SetAccessoryButtons(true);

        CheckIfOrderReady();
    }

    public void SelectWrap2()
    {
        selectedWrap = wrap2;
        wrapSelected = true;

        hybridFlower1.GetComponent<DragReturn>().enabled = false;
        hybridFlower2.GetComponent<DragReturn>().enabled = false;

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

        wrap.GetComponent<DragReturn>().enabled = false;

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

        wrap.GetComponent<DragReturn>().enabled = false;

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
        // =====================
        // ACCESSORY
        // =====================
        if (disposed == accessory1Object || disposed == accessory2Object)
        {
            accessorySelected = false;
            selectedAccessory = null;

            wrap.GetComponent<DragReturn>().enabled = true;

            accessory1Object.SetActive(false);
            accessory1Object.transform.localPosition = accessory1StartPos;

            accessory2Object.SetActive(false);
            accessory2Object.transform.localPosition = accessory2StartPos;

            orderCompleteButton.interactable = false;

            Debug.Log("Accessory removed");
            return;
        }

        // =====================
        // WRAP (only if no accessory)
        // =====================
        if (disposed == wrap.gameObject)
        {
            if (accessorySelected)
            {
                Debug.Log("Remove accessory first!");
                return;
            }

            wrap.GetComponent<DragReturn>().enabled = true;

            wrapSelected = false;
            selectedWrap = null;

            hybridFlower1.GetComponent<DragReturn>().enabled = true;
            hybridFlower2.GetComponent<DragReturn>().enabled = true;

            wrapBackRenderer.sprite = null;
            wrapFrontRenderer.sprite = null;

            SetAccessoryButtons(false);
            orderCompleteButton.interactable = false;

            Debug.Log("Wrap removed");
            return;
        }

        // =====================
        // FLOWER (only if no wrap & no accessory)
        // =====================
        if (disposed == hybridFlower1 || disposed == hybridFlower2)
        {
            if (wrapSelected || accessorySelected)
            {
                Debug.Log("Remove accessory and wrap first!");
                return;
            }

            hybridFlower1.GetComponent<DragReturn>().enabled = true;
            hybridFlower2.GetComponent<DragReturn>().enabled = true;

            ResetPackingScene();
            Debug.Log("Flower removed");
        }
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

        wrapAccessoryTabBackground.SetActive(false);

        // ===== ACCESSORY RESET =====
        accessory1Object.SetActive(false);
        accessory1Object.transform.localPosition = accessory1StartPos;

        accessory2Object.SetActive(false);
        accessory2Object.transform.localPosition = accessory2StartPos;

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

    public void DisposeWholeBouquet()
    {
        Debug.Log("Whole bouquet disposed");

        ResetPackingScene();
    }
}


