using UnityEngine;

public class Bin : MonoBehaviour
{
    [Header("Animation")]
    // Drag your Bin Animator here (must have a bool parameter called "Open")
    public Animator binAnimator;

    [Header("Detection")]
    // The disposable object currently inside the bin trigger
    private GameObject currentDisposable;

    [Header("Hover (2D Raycast)")]
    // Bin collider used for hover detection (assign in Inspector or auto-find)
    public Collider2D binCollider;

    // Track current hover state so we don't spam SetBool every frame
    private bool isHovering;

    void Awake()
    {
        // Auto-grab collider if you forgot to assign it
        if (binCollider == null)
            binCollider = GetComponent<Collider2D>();

        // Start closed
        //CloseBin();
    }

    void Update()
    {
        // If there is no camera, we cannot convert mouse position to world
        if (Camera.main == null || binCollider == null)
            return;

        // Convert mouse screen position to world position
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if mouse is over the bin collider
        bool hoveringNow = binCollider.OverlapPoint(mouseWorld);

        //    // Only update animation when hover state changes
        //    if (hoveringNow != isHovering)
        //    {
        //        isHovering = hoveringNow;

        //        if (isHovering) OpenBin();
        //        else CloseBin();
        //    }
        //}

        void OnTriggerEnter2D(Collider2D other)
        {
            // Only react to objects tagged "Disposable"
            if (!other.CompareTag("Disposable"))
                return;

            // Store the object inside the bin trigger
            currentDisposable = other.gameObject;
        }

        void OnTriggerExit2D(Collider2D other)
        {
            // If the tracked disposable leaves, clear it
            if (currentDisposable == other.gameObject)
            {
                currentDisposable = null;
            }
        }
    }

    // Call this from your input (right click / button) to dispose the object
    public void TryDispose()
    {
        // If nothing is in the bin trigger, do nothing
        if (currentDisposable == null)
            return;

        Debug.Log("[BIN] Disposing: " + currentDisposable.name);

        // If it's a pot, clear its contents
        Pot pot = currentDisposable.GetComponent<Pot>();
        if (pot != null)
        {
            pot.DisposeContents();
        }
        else
        {
            // Otherwise destroy the object
            Destroy(currentDisposable);
        }

        // Clear reference after disposing
        currentDisposable = null;
    }
    

    //void OpenBin()
    //{
    //    if (binAnimator != null)
    //        binAnimator.SetBool("Open", true);
    //}

    //void CloseBin()
    //{
    //    if (binAnimator != null)
    //        binAnimator.SetBool("Open", false);
    //}
}