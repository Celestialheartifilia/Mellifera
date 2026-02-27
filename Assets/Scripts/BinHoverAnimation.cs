using UnityEngine;

public class BinHoverActive : MonoBehaviour
{
    [Header("Hover Target")]
    public Collider2D targetCollider;

    [Header("Objects To Toggle")]
    public GameObject objectToActivate;     // Turns ON when hovering
    public GameObject objectToDeactivate;   // Turns OFF when hovering

    private bool isHovering;

    void Awake()
    {
        if (targetCollider == null)
            targetCollider = GetComponent<Collider2D>();

        // Start state (not hovering)
        if (objectToActivate != null)
            objectToActivate.SetActive(false);

        if (objectToDeactivate != null)
            objectToDeactivate.SetActive(true);
    }

    void Update()
    {
        if (Camera.main == null || targetCollider == null)
            return;

        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        bool hoveringNow = targetCollider.OverlapPoint(mouseWorld);

        if (hoveringNow != isHovering)
        {
            isHovering = hoveringNow;

            if (objectToActivate != null)
                objectToActivate.SetActive(isHovering);

            if (objectToDeactivate != null)
                objectToDeactivate.SetActive(!isHovering);
        }
    }
}