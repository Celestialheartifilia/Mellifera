using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DragReturn : MonoBehaviour
{
    Rigidbody2D rb;
    Camera cam;

    Vector2 offset;
    Vector2 startPos;

    bool dragging;

    public Bin bin;
    public PackingBin packingBin;

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

        // Hybrid Scene Bin
        if (bin != null)
        {
            bin.TryDispose();
        }

        // Packing Scene Bin
        if (packingBin != null)
        {
            packingBin.TryDispose();
        }

        // return back to original position when released
        rb.MovePosition(startPos);
    }

    void FixedUpdate()
    {
        if (!dragging) return;

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 newPos = mouseWorld + offset;

        rb.MovePosition(newPos);
    }
 
}
