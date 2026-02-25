using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DragReturn : MonoBehaviour
{
    Rigidbody2D rb;
    Camera cam;

    Vector2 offset;
    Vector2 startPos;

    bool dragging;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        startPos = rb.position;   // store original position
    }

    void OnMouseDown()
    {
        dragging = true;

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = rb.position - mouseWorld;
    }

    void OnMouseUp()
    {
        dragging = false;

        // return back to original position when released
        rb.MovePosition(startPos);
    }

    void FixedUpdate()
    {
        if (!dragging) return;

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        rb.MovePosition(mouseWorld + offset);
    }
}
