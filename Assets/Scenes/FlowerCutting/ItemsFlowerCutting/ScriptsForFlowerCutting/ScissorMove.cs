using UnityEngine;

public class ScissorMove : MonoBehaviour
{
    Rigidbody2D rb;
    Camera cam;
    Vector2 offset;
    bool dragging;

    Vector2 startPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        startPos = rb.position;
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

    // ? call this to reset scissors
    public void ResetToStart()
    {
        dragging = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.position = startPos;
    }
}
