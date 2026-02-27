using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DragToMove2D : MonoBehaviour
{
    //FOR LEAVES ONLY

    Rigidbody2D rb;
    Camera cam;
    Vector2 offset;
    bool dragging;

    Transform originalParent;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        originalParent = transform.parent;  // store parent
    }

    void OnMouseDown()
    {
        dragging = true;
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = rb.position - mouseWorld;
        transform.parent = null; // detach while dragging
    }

    void OnMouseUp()
    {
        dragging = false;

        transform.parent = originalParent; // reattach
    }

    void FixedUpdate()
    {
        if (!dragging) return;

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        rb.MovePosition(mouseWorld + offset);
    }
}
