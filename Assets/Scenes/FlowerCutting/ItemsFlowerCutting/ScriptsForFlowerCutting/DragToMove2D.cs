using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DragToMove2D : MonoBehaviour
{
    Rigidbody2D rb;
    Camera cam;
    Vector2 offset;
    bool dragging;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        dragging = true;
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = rb.position - mouseWorld;
    }

    void OnMouseUp() => dragging = false;

    void FixedUpdate()
    {
        if (!dragging) return;
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        rb.MovePosition(mouseWorld + offset);
    }
}
